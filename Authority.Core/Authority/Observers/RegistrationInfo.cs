using System;

namespace Authority.Observers
{
    public sealed class RegistrationInfo
    {
        public Guid DomainId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}