using System;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity
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

        public async Task Require(Func<Task<bool>> condition, int errorCode)
        {
            if (!await condition())
            {
                if (Authority.Logger != null)
                {
                    Authority.Logger.Error(string.Format("Operation failed - {0}", errorCode));
                }

                _authorityContext.RollbackTransaction();
                throw new RequirementFailedException(errorCode);
            }
        }

        public void Require(Func<bool> condition, int errorCode)
        {
            if (!condition())
            {
                if (Authority.Logger != null)
                {
                    Authority.Logger.Error(string.Format("Operation failed - {0}", errorCode));
                }

                _authorityContext.RollbackTransaction();
                throw new RequirementFailedException(errorCode);
            }
        }

        public void Update<T>(T entity)
            where T : EntityBase
        {
            _authorityContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task CommitAsync()
        {
            try
            {
                await _authorityContext.SaveChangesAsync();
                _authorityContext.CommitTransaction();
            }
            catch (Exception e)
            {
                if (Authority.Logger != null)
                {
                    Authority.Logger.Error("Operation failed", e);
                }

                _authorityContext.RollbackTransaction();
                throw;
            }
        }

        public virtual void Commit()
        {
            try
            {
                _authorityContext.SaveChanges();
                _authorityContext.CommitTransaction();
            }
            catch (Exception e)
            {
                if (Authority.Logger != null)
                {
                    Authority.Logger.Error("Operation failed", e);
                }

                _authorityContext.RollbackTransaction();
                throw;
            }
        }
    }
}
