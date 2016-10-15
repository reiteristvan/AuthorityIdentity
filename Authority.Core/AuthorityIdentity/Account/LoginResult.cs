using System;
using System.Collections.Generic;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.Account
{
    public sealed class LoginResult
    {
        public LoginResult()
        {
            UserId = Guid.Empty;
            Username = "";
            Email = "";
            WaitForTwoFactor = false;
            Policies = new List<Policy>();
            Claims = new List<AuthorityClaim>();
        }

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTimeOffset LastLogin { get; set; }
        public bool WaitForTwoFactor { get; set; }

        public List<Policy> Policies { get; set; } 
        public List<AuthorityClaim> Claims { get; set; } 
    }
}
