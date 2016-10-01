using System;

namespace AuthorityIdentity
{
    public interface IAuthorityLogger
    {
        void Info(string message);
        void Error(string message);
        void Error(string message, Exception exception);
    }
}
