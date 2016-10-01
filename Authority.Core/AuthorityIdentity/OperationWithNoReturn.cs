using System.Data;
using System.Threading.Tasks;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity
{
    public abstract class OperationWithNoReturn : Operation
    {
        protected OperationWithNoReturn(IAuthorityContext authorityContext,
                            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            : base(authorityContext, isolationLevel)
        {
            
        }

        public abstract void Do();
    }

    public abstract class SafeOperationWithNoReturn : SafeOperation
    {
        protected SafeOperationWithNoReturn(IAuthorityContext authorityContext)
            : base(authorityContext)
        {

        }

        public abstract void Do();
    }

    public abstract class OperationWithNoReturnAsync : Operation
    {
        protected OperationWithNoReturnAsync(IAuthorityContext authorityContext,
                            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            : base(authorityContext, isolationLevel)
        {

        }

        public abstract Task Do();
    }

    public abstract class SafeOperationWithNoReturnAsync : SafeOperation
    {
        protected SafeOperationWithNoReturnAsync(IAuthorityContext authorityContext)
            : base(authorityContext)
        {

        }

        public abstract Task Do();
    }
}
