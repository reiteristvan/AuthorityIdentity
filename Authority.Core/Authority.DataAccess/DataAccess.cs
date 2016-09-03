using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Authority.DataAccess.Scripts;
using Dapper;

namespace Authority.DataAccess
{
    public static class DataAccess
    {
        //public const string ConnectionString = "AuthorityConnection";
        public const string ConnectionStringName = "AuthorityTestConnection";
        private static string ConnectionString;

        public static void Initialize()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

            if (!IsDbExists())
            {
                CreateTables();
            }
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        private static bool IsDbExists()
        {
            using (SqlConnection connection = CreateConnection())
            {
                IEnumerable<dynamic> result = connection.Query(InitializeScripts.CheckIfDbExists);
                return result.Count() == 1;
            }
        }

        private static void CreateTables()
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Execute(InitializeScripts.CreateTables);
            }
        }
    }
}
