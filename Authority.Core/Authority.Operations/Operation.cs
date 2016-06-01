using System;
using System.Data;
using System.Threading.Tasks;
using Authority.EntityFramework;

namespace Authority.Operations
{
    public abstract class Operation
    {
        private readonly IAuthorityContext _authorityContext;

        protected Operation(IAuthorityContext AuthorityContext,
                            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _authorityContext = AuthorityContext;
            _authorityContext.BeginTransaction(isolationLevel);
        }

        protected ISafeAuthorityContext Context { get { return _authorityContext; } }

        public async Task Check(Func<Task<bool>> condition, int errorCode)
        {
            if (!(await condition()))
            {
                _authorityContext.RollbackTransaction();
                throw new RequirementFailedException(errorCode);
            }
        }

        public void Check(Func<bool> condition, int errorCode)
        {
            if (!condition())
            {
                _authorityContext.RollbackTransaction();
                throw new RequirementFailedException(errorCode);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _authorityContext.SaveChangesAsync();
                _authorityContext.CommitTransaction();
            }
            catch (Exception)
            {
                _authorityContext.RollbackTransaction();
                throw;
            }
        }

        public void Commit()
        {
            try
            {
                _authorityContext.SaveChanges();
                _authorityContext.CommitTransaction();
            }
            catch (Exception)
            {
                _authorityContext.RollbackTransaction();
                throw;
            }
        }
    }
}
