using System;

namespace AuthorityIdentity
{
    public sealed class RequirementFailedException : Exception
    {
        public RequirementFailedException(int errorCode, string message = "")
            : base(string.Format("{0} , {1}", message, errorCode), null)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; private set; }
    }
}
