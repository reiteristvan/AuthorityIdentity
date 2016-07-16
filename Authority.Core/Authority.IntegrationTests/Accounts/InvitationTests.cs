using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.IntegrationTests.Common;
using Authority.IntegrationTests.Fixtures;
using Authority.Operations;
using Authority.Operations.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class InvitationTests : IClassFixture<AccountTestFixture>
    {
        private readonly AccountTestFixture _fixture;
        private readonly IAuthorityContext _context;
        private readonly Guid _domainId;

        public InvitationTests(AccountTestFixture fixture)
        {
            _fixture = fixture;

            _context = _fixture.Context;
            _domainId = _fixture.Domain.Id;
        }

        [Fact]
        public async Task InviteAndAcceptShouldSucceed()
        {
            // Arrange
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString();

            // Act
            InviteUser inviteOperation = new InviteUser(_context, email, _domainId);
            Guid invitationCode = inviteOperation.Do();
            await inviteOperation.CommitAsync();

            FinalizeInvitation finalizeOperation = new FinalizeInvitation(_context, invitationCode, username, password);
            await finalizeOperation.Do();
            await finalizeOperation.CommitAsync();

            // Assert
            Invite invite = await _context.Invites.FirstOrDefaultAsync(i => i.Id == invitationCode);
            Assert.NotNull(invite);
            Assert.True(invite.Accepted);

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            Assert.NotNull(user);
            Assert.False(user.IsPending);
        }

        [Fact]
        public async Task InviteAndAcceptWithExpirationShouldSucceed()
        {
            // Arrange
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString();
            DateTimeOffset expire = DateTimeOffset.UtcNow.AddDays(1);

            // Act
            InviteUser inviteOperation = new InviteUser(_context, email, _domainId, expire);
            Guid invitationCode = inviteOperation.Do();
            await inviteOperation.CommitAsync();

            FinalizeInvitation finalizeOperation = new FinalizeInvitation(_context, invitationCode, username, password);
            await finalizeOperation.Do();
            await finalizeOperation.CommitAsync();

            // Assert
            Invite invite = await _context.Invites.FirstOrDefaultAsync(i => i.Id == invitationCode);
            Assert.NotNull(invite);
            Assert.True(invite.Accepted);

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            Assert.NotNull(user);
            Assert.False(user.IsPending);
        }

        [Fact]
        public async Task InviteAndAcceptWithExpiredShouldFail()
        {
            // Arrange
            string email = RandomData.Email();
            string username = RandomData.RandomString();
            string password = RandomData.RandomString();
            DateTimeOffset expire = DateTimeOffset.UtcNow.AddDays(-1);

            // Act
            InviteUser inviteOperation = new InviteUser(_context, email, _domainId, expire);
            Guid invitationCode = inviteOperation.Do();
            await inviteOperation.CommitAsync();

            // Assert
            await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
            {
                FinalizeInvitation finalizeOperation = new FinalizeInvitation(_context, invitationCode, username, password);
                await finalizeOperation.Do();
                await finalizeOperation.CommitAsync();
            }, ex => ex.ErrorCode == AccountErrorCodes.InviteExpired);

        }
    }
}
