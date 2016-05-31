using System;
using System.Collections.Generic;

namespace Authority.Core
{
    public sealed class Policy : EntityBase
    {
        public Policy()
        {
            Claims = new HashSet<Claim>();
            Users = new HashSet<User>();
        }

        public Guid ProductId { get; set; }

        public string Name { get; set; }
        public bool Default { get; set; }

        public ICollection<Claim> Claims { get; set; }
        public ICollection<User> Users { get; set; } 
    }
}
