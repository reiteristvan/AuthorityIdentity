using System;
using System.Threading.Tasks;
using AuthorityIdentity.Account;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.IntegrationTests.Common;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Accounts
{
    public sealed class ActivationTests
    {
        [Fact]
        public async Task ActivationShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                User user = await TestOperations.RegisterUser(testContext.Context, testContext.Domain.Id);

                ActivateUser activation = new ActivateUser(testContext.Context, user.PendingRegistrationId);
                await activation.Do();
                await activation.CommitAsync();

                User activated = testContext.Context.ReloadEntity<User>(user.Id);

                Assert.True(activated.IsPending == false && activated.PendingRegistrationId == Guid.Empty);
            }
        }
    }
}
