using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Xunit;

namespace Authority.IntegrationTests.Policies
{
    public sealed class DefaultPolicyTests : IClassFixture<PolicyTestFixture>
    {
        private readonly PolicyTestFixture _fixture;

        public DefaultPolicyTests(PolicyTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task RegisterUserWithDefaultPolicyShouldSucceed()
        {
            Policy defaultPolicy = await TestOperations.CreatePolicy(
                _fixture.Context, 
                _fixture.Domain.Id,
                RandomData.RandomString(), 
                true);

            User user = await TestOperations.RegisterAndActivateUser(
                _fixture.Context, 
                _fixture.Domain.Id,
                RandomData.RandomString());

            Guid domainId = _fixture.Domain.Id;
            string email = user.Email;

            user = await _fixture.Context.Users
                .Include(u => u.Policies)
                .FirstAsync(u => u.DomainId == domainId && u.Email == email);

            Assert.NotNull(user.Policies);
            Assert.True(user.Policies.Any(p => p.Id == defaultPolicy.Id));
        }
    }
}
