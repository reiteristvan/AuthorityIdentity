using System;
using System.Data;
using System.Data.SqlClient;

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

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
