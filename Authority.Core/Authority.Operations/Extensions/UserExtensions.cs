using System;
using System.Collections.Generic;
using System.Linq;
using Authority.DomainModel;

namespace Authority.Operations.Extensions
{
    public enum EffectiveClaimStrategy
    {
        /// <summary>
        /// Combines all claim from policies and groups
        /// </summary>
        Union = 0,
        
        /// <summary>
        /// Take claims which are both inherited from groups and owned through policies
        /// </summary>
        Intersect = 1,

        Custom = 2
    }

    public interface IClaimStrategy
    {
        List<AuthorityClaim> Collect(List<AuthorityClaim> ownedClaims, List<AuthorityClaim> inheritedClaims);
    }

    public static class UserExtensions
    {
        /// <summary>
        /// Returns the list of claims the user owns through policies.
        /// The list does not contains claims inherited from groups.
        /// The user instance must be acquired through a FindBy* call otherwise the instance won't have the owned policies.
        /// </summary>
        /// <param name="user">A user instance</param>
        /// <returns>List of claims</returns>
        public static List<AuthorityClaim> OwnedClaims(this User user)
        {
            if (user.Policies == null || !user.Policies.Any())
            {
                return new List<AuthorityClaim>();
            }

            List<AuthorityClaim> ownedClaims = new List<AuthorityClaim>();

            foreach (Policy policy in user.Policies)
            {
                if (policy.Claims == null || !policy.Claims.Any())
                {
                    continue;
                }

                IEnumerable<AuthorityClaim> claimsToAdd = policy.Claims.Where(c => !ownedClaims.Contains(c));
                ownedClaims.AddRange(claimsToAdd);
            }

            return ownedClaims;
        }

        /// <summary>
        /// Returns the list of claims which the user has through policies and groups.
        /// The user instance must be acquired through a FindBy* call otherwise the instance won't have the policies and groups.
        /// </summary>
        /// <param name="user">A user instance</param>
        /// <param name="strategy">The strategy by which claims are collected</param>
        /// <param name="customStrategy">Custom claim collector strategy</param>
        /// <returns></returns>
        public static List<AuthorityClaim> EffectiveClaims(this User user, EffectiveClaimStrategy strategy = EffectiveClaimStrategy.Union, IClaimStrategy customStrategy = null)
        {
            List<AuthorityClaim> ownedClaims = user.OwnedClaims();

            if (user.Groups == null || !user.Groups.Any())
            {
                return ownedClaims;
            }

            List<AuthorityClaim> groupClaims = new List<AuthorityClaim>();

            foreach (Group @group in user.Groups)
            {
                IEnumerable<AuthorityClaim> claims = @group.Policies
                    .SelectMany(p => p.Claims)
                    .DistinctBy(c => c.Id)
                    .Where(ac => !groupClaims.Contains(ac));

                groupClaims.AddRange(claims);
            }

            switch (strategy)
            {
                case EffectiveClaimStrategy.Union:
                    return UnionStrategy(ownedClaims, groupClaims);
                case EffectiveClaimStrategy.Intersect:
                    return IntersectStrategy(ownedClaims, groupClaims);
                case EffectiveClaimStrategy.Custom:
                    return customStrategy.Collect(ownedClaims, groupClaims);
            }

            throw new ArgumentException("Unknown claim strategy");
        }

        private static List<AuthorityClaim> UnionStrategy(List<AuthorityClaim> owned, List<AuthorityClaim> inherited)
        {
            return inherited.Union(owned).ToList();
        }

        private static List<AuthorityClaim> IntersectStrategy(List<AuthorityClaim> owned, List<AuthorityClaim> inherited)
        {
            return inherited.Intersect(owned).ToList();
        }
    }
}
