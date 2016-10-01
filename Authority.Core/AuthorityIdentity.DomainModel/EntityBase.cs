using System;

namespace AuthorityIdentity.DomainModel
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; protected set; }
    }
}
