using System;

namespace Authority.DomainModel
{
    public class Developer : EntityBase
    {
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public bool IsPending { get; set; }

        public Guid PendingRegistrationId { get; set; }

        public bool IsActive { get; set; }

        public DateTime Created { get; set; }
    }
}
