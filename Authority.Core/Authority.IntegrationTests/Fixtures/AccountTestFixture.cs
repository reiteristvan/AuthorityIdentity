using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Authority.DomainModel;

namespace Authority.IntegrationTests.Fixtures
{
    public sealed class AccountTestFixture : FixtureBase
    {
        public override void Dispose()
        {
            foreach (DbEntityEntry changedEntry in Context.ChangeTracker.Entries().Where(c => c.Entity is User))
            {
                Context.Entry(changedEntry.Entity).State = EntityState.Deleted;
            }

            Context.SaveChanges();

            foreach (DbEntityEntry changedEntry in Context.ChangeTracker.Entries().Where(c => c.Entity is Product))
            {
                Context.Entry(changedEntry.Entity).State = EntityState.Deleted;
            }

            Context.SaveChanges();

            Context.Dispose();
        }
    }
}
