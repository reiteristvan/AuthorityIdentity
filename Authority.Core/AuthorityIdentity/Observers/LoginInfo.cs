using System;

namespace AuthorityIdentity.Observers
{
    public sealed class LoginInfo
    {
        public Guid DomainId { get; set; }
        public string Email { get; set; }
    }
}