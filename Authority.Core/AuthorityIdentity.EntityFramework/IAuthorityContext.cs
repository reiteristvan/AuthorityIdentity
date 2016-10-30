using System.Data;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace AuthorityIdentity.EntityFramework
{
    public interface IAuthorityContext : ISafeAuthorityContext
    {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;       
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction(IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollbackTransaction();
    }
}