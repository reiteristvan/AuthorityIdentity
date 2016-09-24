using System;

namespace Authority.Observers
{
    public sealed class LoginInfo
    {
        public Guid DomainId { get; set; }
        public string Email { get; set; }
    }
}