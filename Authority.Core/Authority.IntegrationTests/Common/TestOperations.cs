using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Account;
using Authority.Operations.Policies;
using Authority.Operations.Products;

namespace Authority.IntegrationTests.Common
{
    public static class TestOperations
    {
        public static async Task<Domain> CreateDomain(AuthorityContext context)
        {
            CreateDomain operation = new CreateDomain(context, RandomData.RandomString());
            Guid productId = await operation.Do();
            await operation.CommitAsync();

            Domain product = await context.Domains.FirstOrDefaultAsync(p => p.Id == productId);

            return product;
        }

        public static async Task<Policy> CreatePolicy(
            AuthorityContext context, 
            Guid domainId, 
            string name,
            bool defaultPolicy = false)
        {
            CreatePolicy operation = new CreatePolicy(context, domainId, name, defaultPolicy);

            Policy policy = await operation.Do();
            await operation.CommitAsync();

            return policy;
        }

        public static async Task<User> RegisterUser(AuthorityContext context, Guid domainId, string password = "")
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            password = string.IsNullOrEmpty(password) ? RandomData.RandomString(12, true) : password;

            RegisterUser operation = new RegisterUser(context, domainId, email, username, password);
            User user = await operation.Do();
            await operation.CommitAsync();

            return user;
        }

        public static async Task<User> RegisterAndActivateUser(AuthorityContext context, Guid domainId, string password)
        {
            User user = await RegisterUser(context, domainId, password);

            ActivateUser activation = new ActivateUser(context, user.PendingRegistrationId);
            await activation.Do();
            await activation.CommitAsync();

            return user;
        }

        public static async Task<List<User>> CreateUsers(AuthorityContext context, Guid domainId, int numberOfUsers = 10)
        {
            List<User> users = new List<User>();

            for (int i = 0; i < numberOfUsers; ++i)
            {
                User user = await RegisterAndActivateUser(context, domainId, RandomData.RandomString());
                users.Add(user);
            }

            return users;
        }
    }
}
