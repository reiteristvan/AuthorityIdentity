using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Account;
using Authority.Operations.Developers;
using Authority.Operations.Products;

namespace Authority.IntegrationTests.Common
{
    public static class TestOperations
    {
        public static async Task<Developer> RegisterDeveloper(AuthorityContext context, string password = "")
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            password = password == "" ? RandomData.RandomString(12, true) : password;

            DeveloperRegistration operation = new DeveloperRegistration(context, email, username, password);
            Developer developer = await operation.Do();

            await operation.CommitAsync();

            return developer;
        }

        public static async Task<Developer> RegisterAndActivateDeveloper(
            AuthorityContext context,
            string password = "")
        {
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            password = password == "" ? RandomData.RandomString(12, true) : password;

            DeveloperRegistration registration = new DeveloperRegistration(context, email, username, password);
            Developer developer = await registration.Do();
            await registration.CommitAsync();

            DeveloperActivation activation = new DeveloperActivation(context, developer.PendingRegistrationId);
            await activation.Do();
            await activation.CommitAsync();

            return developer;
        }

        public static async Task<Product> CreateProductAndPublish(AuthorityContext context, Guid ownerId)
        {
            CreateProduct operation = new CreateProduct(context, ownerId, RandomData.RandomString(), "", "", "");
            Guid productId = await operation.Do();
            await operation.CommitAsync();

            Product product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            ToggleProductPublish publishOperation = new ToggleProductPublish(context, product.OwnerId, productId);
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
