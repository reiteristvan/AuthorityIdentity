using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.Models.UserModels;

namespace Authority.DataAccess.Repositories
{
    public interface IUserRepository
    {
        
    }

    public sealed class UserRepository : RepositoryBase, IUserRepository
    {
        public UserDetail FindById(Guid userId)
        {
            string sql = "select * from Authority.Users";
            return new UserDetail();
        }
    }
}
