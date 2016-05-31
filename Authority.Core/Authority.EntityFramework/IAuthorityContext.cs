using System.Data;
using System.Threading.Tasks;

namespace Authority.EntityFramework
{
    public interface IAuthorityContext : ISafeAuthorityContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction(IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollbackTransaction();
    }
}