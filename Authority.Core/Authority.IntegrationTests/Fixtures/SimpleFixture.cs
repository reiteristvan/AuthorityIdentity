using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Authority.EntityFramework;

namespace Authority.IntegrationTests.Fixtures
{
    public class SimpleFixture : IDisposable
    {
        public SimpleFixture()
        {
            Context = new AuthorityContext();
        }

        public AuthorityContext Context { get; private set; }

        public void Dispose()
        {
            IEnumerable<DbEntityEntry> changes = Context.ChangeTracker.Entries();

            foreach (DbEntityEntry changedEntry in changes)
            {
                Context.Entry(changedEntry.Entity).State = EntityState.Deleted;
            }

            Context.SaveChanges();
            Context.Dispose();
        }
    }
}
