using System;
using System.Configuration;
using Authority.EntityFramework;

namespace Authority.Operations
{
    public static class AuthorityContextProvider
    {
        public static AuthorityContext Create()
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["AuthorityConnection"];

            if (connectionString == null)
            {
                throw new ArgumentException("Authority ConnectionString");
            }

            return new AuthorityContext();
        }
    }
}
