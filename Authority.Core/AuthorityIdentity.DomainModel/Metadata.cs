using System;

namespace AuthorityIdentity.DomainModel
{
    public sealed class Metadata : EntityBase
    {
        public const string TableName = "Authority.Metadata";

        public Metadata()
        {
            
        }

        public Metadata(Guid userId)
            : base(userId)
        {
            
        }

        public string Data { get; set; }
    }
}
