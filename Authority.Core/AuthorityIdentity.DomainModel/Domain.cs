using System.Collections.Generic;

namespace AuthorityIdentity.DomainModel
{
    public class Domain : EntityBase
    {
        public const string TableName = "Authority.Domains";

        public Domain()
        {
            Groups = new HashSet<Group>();
            Policies = new HashSet<Policy>();
            Claims = new HashSet<AuthorityClaim>();
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Group> Groups { get; set; } 
        public ICollection<Policy> Policies { get; set; }
        public ICollection<AuthorityClaim> Claims { get; set; }
    }
}
