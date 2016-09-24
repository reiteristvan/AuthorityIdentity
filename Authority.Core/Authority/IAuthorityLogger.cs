using System;

namespace Authority
{
    public interface IAuthorityLogger
    {
        void Info(string message);
        void Error(string message);
        void Error(string message, Exception exception);
    }
}
