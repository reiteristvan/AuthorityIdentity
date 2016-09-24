using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority;
using Authority.Account;
using Xunit;

namespace Authority.IntegrationTests.Accounts
{
    public sealed class InvitationTests
    {
        [Fact]
        public async Task InviteAndAcceptShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string email = RandomData.Email();
                string username = RandomData.RandomString();
                string password = RandomData.RandomString();

                // Act
                InviteUser inviteOperation = new InviteUser(testContext.Context, email, testContext.Domain.Id);
                Guid invitationCode = inviteOperation.Do();
                await inviteOperation.CommitAsync();

                FinalizeInvitation finalizeOperation = new FinalizeInvitation(testContext.Context, invitationCode, username,
                    password);
                await finalizeOperation.Do();
                await finalizeOperation.CommitAsync();

                // Assert
                Invite invite = await testContext.Context.Invites.FirstOrDefaultAsync(i => i.Id == invitationCode);
                Assert.NotNull(invite);
                Assert.True(invite.Accepted);

                User user = await testContext.Context.Users.FirstOrDefaultAsync(u => u.Email == email);
                Assert.NotNull(user);
                Assert.False(user.IsPending);
            }
        }

        [Fact]
        public async Task InviteAndAcceptWithExpirationShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string email = RandomData.Email();
                string username = RandomData.RandomString();
                string password = RandomData.RandomString();
                DateTimeOffset expire = DateTimeOffset.UtcNow.AddDays(1);

                // Act
                InviteUser inviteOperation = new InviteUser(testContext.Context, email, testContext.Domain.Id, expire);
                Guid invitationCode = inviteOperation.Do();
                await inviteOperation.CommitAsync();

                FinalizeInvitation finalizeOperation = new FinalizeInvitation(testContext.Context, invitationCode, username,
                    password);
                await finalizeOperation.Do();
                await finalizeOperation.CommitAsync();

                // Assert
                Invite invite = await testContext.Context.Invites.FirstOrDefaultAsync(i => i.Id == invitationCode);
                Assert.NotNull(invite);
                Assert.True(invite.Accepted);

                User user = await testContext.Context.Users.FirstOrDefaultAsync(u => u.Email == email);
                Assert.NotNull(user);
                Assert.False(user.IsPending);
            }
        }

        [Fact]
        public async Task InviteAndAcceptWithExpiredShouldFail()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                // Arrange
                string email = RandomData.Email();
                string username = RandomData.RandomString();
                string password = RandomData.RandomString();
                DateTimeOffset expire = DateTimeOffset.UtcNow.AddDays(-1);

                // Act
                InviteUser inviteOperation = new InviteUser(testContext.Context, email, testContext.Domain.Id, expire);
                Guid invitationCode = inviteOperation.Do();
                await inviteOperation.CommitAsync();

                // Assert
                await AssertExtensions.ThrowAsync<RequirementFailedException>(async () =>
                {
                    FinalizeInvitation finalizeOperation = new FinalizeInvitation(testContext.Context, invitationCode, username,
                        password);
                    await finalizeOperation.Do();
                    await finalizeOperation.CommitAsync();
                }, ex => ex.ErrorCode == ErrorCodes.InviteExpired);
            }
        }
    }
}
