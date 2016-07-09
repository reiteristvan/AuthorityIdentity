using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class BulkRegistrationTest : IClassFixture<AccountTestFixture>
    {
        private readonly AccountTestFixture _fixture;

        public BulkRegistrationTest(AccountTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task BulkRegisterShouldSucceed()
        {
            List<BulkRegistrationData> registrationDataList = new List<BulkRegistrationData>();

            for (int i = 0; i < 100; ++i)
            {
                BulkRegistrationData registrationData = new BulkRegistrationData
                {
                    DomainId = _fixture.Domain.Id,
                    Email = RandomData.Email(),
                    Password = RandomData.RandomString(12, true),
                    Username = RandomData.RandomString()
                };

                registrationDataList.Add(registrationData);
            }

            BulkUserRegistration operation = new BulkUserRegistration(_fixture.Context, registrationDataList, true);
            await operation.Execute();
        }
    }
}
