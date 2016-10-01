using System;
using System.Collections.Generic;

namespace AuthorityIdentity.DomainModel
{
    public sealed class Policy : EntityBase
    {
        public const string TableName = "Authority.Policies";

        public Policy()
        {
            Claims = new HashSet<AuthorityClaim>();
            Users = new HashSet<User>();
            Groups = new HashSet<Group>();
        }

        public Guid DomainId { get; set; }
        public string Name { get; set; }
        public bool Default { get; set; }

        public ICollection<AuthorityClaim> Claims { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Group> Groups { get; set; }  
    }
}
