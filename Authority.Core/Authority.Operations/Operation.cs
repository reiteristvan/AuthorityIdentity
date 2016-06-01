using System;
using System.Data;
using System.Threading.Tasks;
using Authority.EntityFramework;
using Serilog.Events;

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
            if (!await condition())
            {
                Authority.Logger.Write(LogEventLevel.Error, "Operation failed - {0}", errorCode);
                _authorityContext.RollbackTransaction();
                throw new RequirementFailedException(errorCode);
            }
        }

        public void Check(Func<bool> condition, int errorCode)
        {
            if (!condition())
            {
                Authority.Logger.Write(LogEventLevel.Error, "Operation failed - {0}", errorCode);
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
            catch (Exception e)
            {
                Authority.Logger.Write(LogEventLevel.Error, "Operation failed - {0}", e.StackTrace);
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
            catch (Exception e)
            {
                Authority.Logger.Write(LogEventLevel.Error, "Operation failed - {0}", e.StackTrace);
                _authorityContext.RollbackTransaction();
                throw;
            }
        }
    }
}
