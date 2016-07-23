using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations.Policies;
using Xunit;

namespace Authority.IntegrationTests.Policies
{
    public sealed class DeleteTests : IClassFixture<PolicyTestFixture>
    {
        private readonly PolicyTestFixture _fixture;
        private readonly IAuthorityContext _context;
        private readonly Guid _domainId;

        public DeleteTests(PolicyTestFixture fixture)
        {
            _fixture = fixture;

            _context = _fixture.Context;
            _domainId = _fixture.Domain.Id;
        }

        [Fact]
        public async Task DeletePolicyShouldSucceed()
        {
            // Arrange
            CreatePolicy create = new CreatePolicy(_context, _domainId, RandomData.RandomString());
            Policy policy = await create.Do();
            await create.CommitAsync();

            // Act
            DeletePolicy delete = new DeletePolicy(_context, policy.Id);
            await delete.Do();
            await delete.CommitAsync();

            // Assert
            policy = (_context as DbContext).ReloadEntity<Policy>(policy.Id);

            Assert.Null(policy);
        }

        [Fact]
        public async Task AddUserToPolicyThenDeleteUserShouldRemain()
        {
            // Arrange
            Policy policy = await TestOperations.CreatePolicy(_context as AuthorityContext, _domainId, RandomData.RandomString(), true);
            List<User> users = await TestOperations.CreateUsers(_context as AuthorityContext, _domainId);

            // Act
            DeletePolicy delete = new DeletePolicy(_context, policy.Id);
            await delete.Do();
            await delete.CommitAsync();

            // Assert
            policy = (_context as DbContext).ReloadEntity<Policy>(policy.Id);
            Assert.Null(policy);

            foreach (User user in users)
            {
                User reloaded = (_context as DbContext).ReloadEntity<User>(user.Id);
                Assert.NotNull(reloaded);
            }
        }
    }
}
