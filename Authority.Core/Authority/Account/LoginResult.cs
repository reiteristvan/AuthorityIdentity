using System;
using System.Collections.Generic;
using Authority.DomainModel;

namespace Authority.Account
{
    public sealed class LoginResult
    {
        public LoginResult()
        {
            Username = "";
            Email = "";
            Policies = new List<Policy>();
            Claims = new List<AuthorityClaim>();
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public DateTimeOffset LastLogin { get; set; }

        public List<Policy> Policies { get; set; } 
        public List<AuthorityClaim> Claims { get; set; } 
    }
}
