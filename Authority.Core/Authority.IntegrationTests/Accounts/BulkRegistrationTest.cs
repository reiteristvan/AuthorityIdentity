using System.Collections.Generic;
using System.Threading.Tasks;
using AuthorityIdentity.Account;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Accounts
{
    public sealed class BulkRegistrationTest
    {
        [Fact(Skip = "100 users takes ~30 seconds")]
        public async Task BulkRegisterShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                List<BulkRegistrationData> registrationDataList = new List<BulkRegistrationData>();

                for (int i = 0; i < 100; ++i)
                {
                    BulkRegistrationData registrationData = new BulkRegistrationData
                    {
                        DomainId = testContext.Domain.Id,
                        Email = RandomData.Email(),
                        Password = RandomData.RandomString(12, true),
                        Username = RandomData.RandomString(),
                        TwoFactorEnabled = false
                    };

                    registrationDataList.Add(registrationData);
                }

                BulkUserRegistration operation = new BulkUserRegistration(testContext.Context, registrationDataList, true);
                await operation.Execute();
            }
        }
    }
}
