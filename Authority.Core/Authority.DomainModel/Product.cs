using System;
using System.Collections.Generic;

namespace Authority.DomainModel
{
    public class Product : EntityBase
    {
        public Product()
        {
            Policies = new HashSet<Policy>();
            Claims = new HashSet<AuthorityClaim>();
        }

        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public string SiteUrl { get; set; }
        public string NotificationEmail { get; set; }
        public string ActivationUrl { get; set; }
        public Guid ApiKey { get; set; }
        public virtual ProductStyle Style { get; set; }

        public ICollection<Policy> Policies { get; set; }
        public ICollection<AuthorityClaim> Claims { get; set; }
    }
}
