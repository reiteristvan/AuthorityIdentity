using System;
using System.Data;
using System.Threading.Tasks;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity
{
    public abstract class SqlOperation
    {
        protected SqlOperation(IAuthorityContext context, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            Context = context;

            Context.BeginTransaction(isolationLevel);
        }

        protected IAuthorityContext Context { get; }

        protected abstract Task Do();

        public async Task Execute()
        {
            try
            {
                await Do();
            }
            catch (Exception e)
            {
                Context.RollbackTransaction();
                Authority.Logger.Error("Operation failed", e);
                throw;
            }

            Context.CommitTransaction();
        }
    }
}
