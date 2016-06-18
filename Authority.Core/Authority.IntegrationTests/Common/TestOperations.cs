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
        public static async Task<Product> CreateProductAndPublish(AuthorityContext context)
        {
            CreateProduct operation = new CreateProduct(context, RandomData.RandomString(), "", "", "");
            Guid productId = await operation.Do();
            await operation.CommitAsync();

            Product product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            ToggleProductPublish publishOperation = new ToggleProductPublish(context, productId);
            await publishOperation.Do();
            await publishOperation.CommitAsync();

            return product;
        }

        public static async Task<User> RegisterUser(AuthorityContext context, Guid productId, string password = "")
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            password = string.IsNullOrEmpty(password) ? RandomData.RandomString(12, true) : password;

            UserRegistration operation = new UserRegistration(context, productId, email, username, password);
            User user = await operation.Do();
            await operation.CommitAsync();

            return user;
        }

        public static async Task<User> RegisterAndActivateUser(AuthorityContext context, Guid productId, string password)
        {
            User user = await RegisterUser(context, productId, password);

            UserActivation activation = new UserActivation(context, productId, user.PendingRegistrationId);
            await activation.Do();
            await activation.CommitAsync();

            return user;
        }
    }
}
