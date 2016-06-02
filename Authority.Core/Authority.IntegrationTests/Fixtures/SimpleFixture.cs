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
                Context.Entry(changedEntry.Entity).State = EntityState.Deleted;
            }

            Context.SaveChanges();
            Context.Dispose();
        }
    }
}
