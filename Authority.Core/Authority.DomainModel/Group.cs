using System;
using System.Collections.Generic;

namespace Authority.DomainModel
{
    public sealed class Group : EntityBase
    {
        public const string TableName = "Authority.Groups";

        public Group()
        {
            Users = new HashSet<User>();
            Policies = new HashSet<Policy>();
        }

        public Guid DomainId { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
        public bool IsActive { get; set; }
        public bool Default { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Policy> Policies { get; set; }  
    }
}
