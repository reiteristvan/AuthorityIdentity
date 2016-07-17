using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations.Policies;
using Xunit;

namespace Authority.IntegrationTests.Policies
{
    public sealed class DefaultPolicyTests : IClassFixture<PolicyTestFixture>
    {
        private readonly IAuthorityContext _context;
        private readonly Guid _domainId;
        private readonly PolicyTestFixture _fixture;

        public DefaultPolicyTests(PolicyTestFixture fixture)
        {
            _fixture = fixture;

            _context = _fixture.Context;
            _domainId = _fixture.Domain.Id;
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

        [Fact]
        public async Task CreateDefaultPolicyWithExistingDefaultShouldSucceed()
        {
            // Arrange
            string firstPolicyName = RandomData.RandomString();
            string secondPolicyName = RandomData.RandomString();

            CreatePolicy createFirstOperation = new CreatePolicy(_context, _domainId, firstPolicyName, true);
            Policy first = await createFirstOperation.Do();
            await createFirstOperation.CommitAsync();

            // Act
            CreatePolicy createSecondOperation = new CreatePolicy(_context, _domainId, secondPolicyName, true, true);
            Policy second = await createSecondOperation.Do();
            await createSecondOperation.CommitAsync();

            // Assert
            first = (_context as DbContext).ReloadEntity<Policy>(first.Id);

            Assert.True(second.Default);
            Assert.False(first.Default);
        }
    }
}
