using System;
using System.Collections.Generic;

namespace Authority.Core
{
    public class Developer : EntityBase
    {
        public Developer()
        {
            Applications = new HashSet<ClientApplication>();
        }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public bool IsPending { get; set; }

        public Guid PendingRegistrationId { get; set; }

        public bool IsActive { get; set; }

        public DateTime Created { get; set; }

        public ICollection<ClientApplication> Applications { get; set; } 
    }
}
