using System.Data;
using System.Threading.Tasks;
using Authority.EntityFramework;

namespace Authority.Operations
{
    public abstract class OperationWithReturnValue<TReturn> : Operation
    {
        protected OperationWithReturnValue(IAuthorityContext authorityContext,
                            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            : base(authorityContext, isolationLevel)
        {
            
        }

        public abstract TReturn Do();
    }

    public abstract class SafeOperationWithReturnValue<TReturn> : SafeOperation
    {
        protected SafeOperationWithReturnValue(ISafeAuthorityContext authorityContext)
            : base(authorityContext)
        {

        }

        public abstract TReturn Do();
    }

    public abstract class OperationWithReturnValueAsync<TReturn> : Operation
    {
        protected OperationWithReturnValueAsync(IAuthorityContext authorityContext,
                            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            : base(authorityContext, isolationLevel)
        {

        }

        public abstract Task<TReturn> Do();
    }

    public abstract class SafeOperationWithReturnValueAsync<TReturn> : SafeOperation
    {
        protected SafeOperationWithReturnValueAsync(ISafeAuthorityContext authorityContext)
            : base(authorityContext)
        {

        }

        public abstract Task<TReturn> Do();
    }
}
