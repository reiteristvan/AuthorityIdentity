using System;

namespace Authority.Models.UserModels
{
    public class User : ModelBase
    {
        public Guid DomainId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public DateTimeOffset LastLogin { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public bool IsPending { get; set; }

        public Guid PendingRegistrationId { get; set; }

        public bool IsExternal { get; set; }

        public string ExternalProviderName { get; set; }

        public string ExternalToken { get; set; }

        public bool IsActive { get; set; }
    }
}
