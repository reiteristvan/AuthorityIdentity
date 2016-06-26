using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class DeleteTests : IClassFixture<AccountTestFixture>
    {
        private readonly AccountTestFixture _fixture;

        public DeleteTests(AccountTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DeleteUserShouldSucceed()
        {
            User user = await TestOperations.RegisterAndActivateUser(
                _fixture.Context, 
                _fixture.Domain.Id,
                RandomData.RandomString());

            DeleteUser operation = new DeleteUser(_fixture.Context, _fixture.Domain.Id, user.Email);
            await operation.Do();
            await operation.CommitAsync();

            Guid domainId = _fixture.Domain.Id;
            string email = user.Email;

            user = await _fixture.Context.Users
                .FirstOrDefaultAsync(u => u.DomainId == domainId && u.Email == email);

            Assert.Null(user);
        }
    }
}
