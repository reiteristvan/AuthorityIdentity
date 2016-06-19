using System;

namespace Authority.Operations.Observers
{
    public sealed class LoginInfo
    {
        public Guid ProductId { get; set; }
        public string Email { get; set; }
    }
}