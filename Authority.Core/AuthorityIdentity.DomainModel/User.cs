using System;
using System.Collections.Generic;

namespace AuthorityIdentity.DomainModel
{
    public enum TwoFactorType
    {
        Email = 1,
        Text = 2,
        App = 3,
        Other = 4
    }

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

        public bool IsTwoFactorEnabled { get; set; }

        public string TwoFactorToken { get; set; }

        public TwoFactorType TwoFactorType { get; set; }

        public string TwoFactorTarget { get; set; }

        public string Metadata { get; set; }

        public ICollection<Group> Groups { get; set; } 
        public ICollection<Policy> Policies { get; set; }
    }
}
