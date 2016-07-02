using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Authority.IntegrationTests.Fixtures
{
    public class SimpleFixture : FixtureBase
    {
        public override void Dispose()
        {
            IEnumerable<DbEntityEntry> changes = Context.ChangeTracker.Entries();

            foreach (DbEntityEntry changedEntry in changes)
            {
                changedEntry.Reload();

                // if the test deleted the entity it will reload with the state 'Detached'
                if (changedEntry.State == EntityState.Detached)
                {
                    continue;
                }

                Context.Entry(changedEntry.Entity).State = EntityState.Deleted;
            }

            Context.SaveChanges();
            Context.Dispose();
        }
    }
}
