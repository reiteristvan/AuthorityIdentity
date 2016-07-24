using System;

namespace Authority.Operations
{
    public interface IAuthorityLogger
    {
        void Info(string message);
        void Error(string message);
        void Error(string message, Exception exception);
    }
}
