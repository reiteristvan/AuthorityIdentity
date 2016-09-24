using System;
using System.Collections.Generic;
using Authority.Models.UserModels;
using Dapper;

namespace Authority.DataAccess.Stores
{
    public interface IUserStore
    {
        IEnumerable<User> All(Guid domainId);
        User FindById(Guid userId);
        User FindByIdWithSecurityEntries(Guid userId);
        User FindByEmail(string email);
        User FindByEmailWithSecurityEntries(string email);
    }

    public sealed class UserStore : IUserStore
    {
        public IEnumerable<User> All(Guid domainId)
        {
            string sql = @"
select DomainId, Email, Username, PasswordHash, Salt, IsPending, PendingRegistrationId, IsActive, Id, LastLogin 
from Authority.Users
where DomaindId = @DomainId";

            IEnumerable<User> users = DataAccess.CreateConnection().Query<User>(sql, new {DomainId = domainId});
            return users;
        }

        public User FindById(Guid userId)
        {
            string sql = @"
select DomainId, Email, Username, PasswordHash, Salt, IsPending, PendingRegistrationId, IsActive, Id, LastLogin 
from Authority.Users
where Id = @Id";

            User user = DataAccess.CreateConnection().QueryFirst<User>(sql, new {Id = userId});
            return user;
        }

        User IUserStore.FindByIdWithSecurityEntries(Guid userId)
        {
            return FindByIdWithSecurityEntries(userId);
        }

        public User FindByEmail(string email)
        {
            string sql = @"
select DomainId, Email, Username, PasswordHash, Salt, IsPending, PendingRegistrationId, IsActive, Id, LastLogin 
from Authority.Users
where Email = @Email";

            User user = DataAccess.CreateConnection().QueryFirst<User>(sql, new {Email = email});
            return user;
        }

        public User FindByEmailWithSecurityEntries(string email)
        {
            throw new NotImplementedException();
        }

        public UserDetail FindByIdWithSecurityEntries(Guid userId)
        {
            string sql = @"
select DomainId, Email, Username, PasswordHash, Salt, IsPending, PendingRegistrationId, IsActive, Id, LastLogin 
from Authority.Users
where Id = @Id";

            UserDetail user = DataAccess.CreateConnection().QueryFirst<UserDetail>(sql, new { Id = userId });
            return user;
        }
    }
}
