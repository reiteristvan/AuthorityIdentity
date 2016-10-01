using System;
using System.Configuration;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity
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
