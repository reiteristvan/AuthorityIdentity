using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.Domains
{
    public sealed class DeleteDomain : SqlOperation
    {
        private readonly Guid _domainId;

        public DeleteDomain(IAuthorityContext AuthorityContext, Guid domainId)
            : base(AuthorityContext)
        {
            _domainId = domainId;
        }

        protected override async Task Do()
        {
            Domain domain = await Context.Domains
                .Include(d => d.Groups)
                .Include(d => d.Policies)
                .FirstOrDefaultAsync(d => d.Id == _domainId);
            
            if (domain == null)
            {
                throw new RequirementFailedException(ErrorCodes.DomainNotExists, _domainId.ToString());
            }

            // Remove UserGroups linker table
            await Context.Database.ExecuteSqlCommandAsync(
                "delete from Authority.UserGroups where User_DomainId = @DomainId",
                new SqlParameter("@DomainId", domain.Id));

            // Remove Groups
            await Context.Database.ExecuteSqlCommandAsync(
                "delete from Authority.Groups where DomainId = @DomainId",
                new SqlParameter("@DomainId", domain.Id));

            // Remove Policies
            foreach (Policy policy in domain.Policies)
            {
                await Context.Database.ExecuteSqlCommandAsync(
                    "delete from Authority.PolicyAuthorityClaims where Policy_Id = @PolicyId",
                    new SqlParameter("@PolicyId", policy.Id));
            }

            //await Context.Database.ExecuteSqlCommandAsync("", new SqlParameter(@"UserId"))

            // Remove users
            await Context.Database.ExecuteSqlCommandAsync(
                "delete from Authority.Users where DomainId = @DomainId",
                new SqlParameter("@DomainId", _domainId));

            // Remove invites
            await Context.Database.ExecuteSqlCommandAsync(
                "delete from Authority.Invites where DomainId = @DomainId",
                new SqlParameter("@DomainId", _domainId));

            // Remove Domain
            await Context.Database.ExecuteSqlCommandAsync(
                "delete from Authority.Domains where Id = @Id",
                new SqlParameter("@Id", _domainId));
        }
    }
}
