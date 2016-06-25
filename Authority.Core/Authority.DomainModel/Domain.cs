using System.Collections.Generic;

namespace Authority.DomainModel
{
    public class Domain : EntityBase
    {
        public Domain()
        {
            Policies = new HashSet<Policy>();
            Claims = new HashSet<AuthorityClaim>();
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Policy> Policies { get; set; }
        public ICollection<AuthorityClaim> Claims { get; set; }
    }
}
