using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using AuthorityIdentity.Account;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Accounts
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
