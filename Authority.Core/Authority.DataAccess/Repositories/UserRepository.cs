using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.Models.UserModels;
using Dapper;

namespace Authority.DataAccess.Repositories
{
    public interface IUserRepository
    {
        
    }

    public sealed class UserRepository : RepositoryBase, IUserRepository
    {
        public User FindById(Guid userId)
        {
            string sql = @"
select DomainId, Email, Username, PasswordHash, Salt, IsPending, PendingRegistrationId, IsActive, Id, LastLogin 
from Authority.Users
where Id = @Id";

            User user = Connection.QueryFirst<User>(sql, new {Id = userId});
            return user;
        }

        public UserDetail FindByIdWithSecurityEntries(Guid userId)
        {
            string sql = @"
select DomainId, Email, Username, PasswordHash, Salt, IsPending, PendingRegistrationId, IsActive, Id, LastLogin 
from Authority.Users
where Id = @Id";

            UserDetail user = Connection.QueryFirst<UserDetail>(sql, new { Id = userId });
            return user;
        }
    }
}
