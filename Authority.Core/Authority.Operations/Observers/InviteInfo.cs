using System;

namespace Authority.Operations.Observers
{
    public sealed class InviteInfo
    {
        public string Email { get; set; }
        public Guid DomainId { get; set; }
    }
}