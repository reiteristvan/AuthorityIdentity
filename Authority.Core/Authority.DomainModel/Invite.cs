using System;

namespace Authority.DomainModel
{
    public sealed class Invite : EntityBase
    {
        public const string TableName = "Authority.Invites";

        public string Email { get; set; }
        public Guid DomainId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Expire { get; set; }
        public bool Accepted { get; set; }
    }
}
