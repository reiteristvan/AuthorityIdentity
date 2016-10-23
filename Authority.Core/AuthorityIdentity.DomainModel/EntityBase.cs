using System;

namespace AuthorityIdentity.DomainModel
{
    public abstract class EntityBase
    {
        protected EntityBase(Guid id = new Guid())
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
        }

        public Guid Id { get; protected set; }
    }
}
