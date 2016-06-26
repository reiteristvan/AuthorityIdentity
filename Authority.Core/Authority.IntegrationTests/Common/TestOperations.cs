using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Account;
using Authority.Operations.Products;

namespace Authority.IntegrationTests.Common
{
    public static class TestOperations
    {
        public static async Task<Domain> CreateDomain(AuthorityContext context)
        {
            CreateProduct operation = new CreateProduct(context, RandomData.RandomString());
            Guid productId = await operation.Do();
            await operation.CommitAsync();

            Domain product = await context.Domains.FirstOrDefaultAsync(p => p.Id == productId);

            return product;
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

            ActivateUser activation = new ActivateUser(context, domainId, user.PendingRegistrationId);
            await activation.Do();
            await activation.CommitAsync();

            return user;
        }
    }
}
