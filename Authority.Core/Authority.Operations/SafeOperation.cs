using System;
using System.Threading.Tasks;
using Authority.EntityFramework;

namespace Authority.Operations
{
    public abstract class SafeOperation
    {
        private readonly ISafeAuthorityContext _authorityContext;

        protected SafeOperation(ISafeAuthorityContext authorityContext)
        {
            _authorityContext = authorityContext;
        }

        public async Task Check(Func<Task<bool>> condition, int errorCode)
        {
            if (!await condition())
            {
                throw new RequirementFailedException(errorCode);
            }
        }

        public void Check(Func<bool> condition, int errorCode)
        {
            if (!condition())
            {
                throw new RequirementFailedException(errorCode);
            }
        }

        protected ISafeAuthorityContext Context { get {  return _authorityContext; } }
    }
}
