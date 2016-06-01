using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Authority.DomainModel;

namespace Authority.IntegrationTests.Common
{
    public static class DbContextExtensions
    {
        public static TEntity FindEntityInChangeTracker<TEntity>(this DbContext context, Guid id)
            where TEntity : EntityBase
        {
            IEnumerable<DbEntityEntry> entries = context.ChangeTracker.Entries();
            EntityBase entity = entries.Select(e => e.Entity).OfType<EntityBase>().First(e => e.Id == id);
            return entity as TEntity;
        }

        public static TEntity ReloadEntity<TEntity>(this DbContext context, Guid id)
            where TEntity : EntityBase
        {
            IEnumerable<DbEntityEntry> entries = context.ChangeTracker.Entries();
            DbEntityEntry entry = entries.First(e => (e.Entity as EntityBase).Id == id);
            entry.Reload();
            return entry.Entity as TEntity;
        }
    }
}
