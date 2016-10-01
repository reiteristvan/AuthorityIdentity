using System.Collections.Generic;

namespace AuthorityIdentity.DomainModel
{
    public sealed class AuthorityClaim : EntityBase
    {
        public const string TableName = "Authority.Claims";

        public AuthorityClaim()
        {
            Policies = new HashSet<Policy>();
        }

        public string FriendlyName { get; set; }

        public string Issuer { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public ICollection<Policy> Policies { get; set; }
    }
}
