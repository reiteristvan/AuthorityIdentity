using System;
using System.Collections.Generic;

namespace Authority.DomainModel
{
    public class User : EntityBase
    {
        public const string TableName = "Authority.Users";

        public User()
        {
            Groups = new HashSet<Group>();
            Policies = new HashSet<Policy>();
        }

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

        public ICollection<Group> Groups { get; set; } 
        public ICollection<Policy> Policies { get; set; }
    }
}
