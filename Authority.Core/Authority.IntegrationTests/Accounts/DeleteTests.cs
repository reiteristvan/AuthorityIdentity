using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Operations.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class DeleteTests
    {
        [Fact]
        public async Task DeleteUserShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                User user = await TestOperations.RegisterAndActivateUser(
                    testContext.Context,
                    testContext.Domain.Id,
                    RandomData.RandomString());

                DeleteUser operation = new DeleteUser(testContext.Context, user.Id);
                await operation.Do();
                await operation.CommitAsync();

                Guid domainId = testContext.Domain.Id;
                string email = user.Email;

                user = await testContext.Context.Users
                    .FirstOrDefaultAsync(u => u.DomainId == domainId && u.Email == email);

                Assert.Null(user);
            }
        }
    }
}
