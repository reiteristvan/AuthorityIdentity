using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Authority.DataAccess.Repositories
{
    public abstract class RepositoryBase : IDisposable
    {
        private SqlTransaction _transaction;

        protected RepositoryBase()
        {
            Connection = DataAccess.CreateConnection();
        }

        protected SqlConnection Connection { get; }

        protected void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = Connection.BeginTransaction(isolationLevel);
        }

        protected void CloseTransaction()
        {
            _transaction?.Commit();
        }

        public T Query<T>(string sql)
        {
            return default(T);
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
