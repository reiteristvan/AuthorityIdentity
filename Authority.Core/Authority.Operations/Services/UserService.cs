using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Account;
using Authority.Operations.Policies;

namespace Authority.Operations.Services
{
    public interface IUserService
    {
        Task<User> FindByEmail(string email, Guid domainId = new Guid());
        Task<User> FindById(Guid id);
        Task<User> Register(string email, string username, string password, bool needToActivate = false, Guid domainId = new Guid());
        Task Acivate(Guid activationCode);
        Task<LoginResult> Login(string email, string password, Guid domainId = new Guid());
        Task Delete(Guid userId);
        Task SetStatus(Guid userId, bool isActive);
        Task AddToPolicy(Guid userId, Guid policyId);
        Task RemoveFromPolicy(Guid userId, Guid policyId);
        Task BulkRegistration(List<BulkRegistrationData> registrationData, bool shouldActivate = false);
        Guid Invite(string email, DateTimeOffset? expireOn = null, Guid domainId = new Guid());
        Task FinalizeInvitation(Guid invitationCode, string username, string password);
    }

    public sealed class UserService : IUserService
    {
        public async Task<User> FindByEmail(string email, Guid domainId = new Guid())
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email");
            }

            IAuthorityContext context = AuthorityContextProvider.Create();

            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            User user = await context.Users
                .Include(u => u.Groups)
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(p => p.Claims))
                .FirstOrDefaultAsync(u => u.Email == email && u.DomainId == domainId);

            return user;
        }

        public async Task<User> FindById(Guid id)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            User user = await context.Users
                .Include(u => u.Groups)
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(p => p.Claims))
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> Register(string email, string username, string password, bool needToActivate = false, Guid domainId = new Guid())
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("One or more argument is invalid");
            }

            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();
            RegisterUser registerOperation = new RegisterUser(context, domainId, email, username, password, needToActivate);
            User user = await registerOperation.Do();
            await registerOperation.CommitAsync();

            return user;
        }

        public async Task Acivate(Guid activationCode)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            ActivateUser activateOperation = new ActivateUser(context, activationCode);
            await activateOperation.Do();
            await activateOperation.CommitAsync();
        }

        public async Task<LoginResult> Login(string email, string password, Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();

            LoginUser loginOperation = new LoginUser(context, domainId, email, password);
            LoginResult result = await loginOperation.Do();
            await loginOperation.CommitAsync();

            return result;
        }

        public async Task Delete(Guid userId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            DeleteUser deleteOperation = new DeleteUser(context, userId);
            await deleteOperation.Do();
            await deleteOperation.CommitAsync();
        }

        public async Task SetStatus(Guid userId, bool isActive)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            SetUserStatus setStatus = new SetUserStatus(context, userId, isActive);
            await setStatus.Do();
            await setStatus.CommitAsync();
        }

        public async Task AddToPolicy(Guid userId, Guid policyId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            AddUserToPolicy addToPolicy = new AddUserToPolicy(context, userId, policyId);
            await addToPolicy.Do();
            await addToPolicy.CommitAsync();
        }

        public async Task RemoveFromPolicy(Guid userId, Guid policyId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            RemoveUserFromPolicy removeFromPolicy = new RemoveUserFromPolicy(context, userId, policyId);
            await removeFromPolicy.Do();
            await removeFromPolicy.CommitAsync();
        }

        public async Task BulkRegistration(List<BulkRegistrationData> registrationData, bool shouldActivate = false)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            BulkUserRegistration bulkRegistration = new BulkUserRegistration(context, registrationData, shouldActivate);
            await bulkRegistration.Execute();
        }

        public Guid Invite(string email, DateTimeOffset? expireOn = null, Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();
            InviteUser invite = new InviteUser(context, email, domainId, expireOn);
            Guid invitationCode =  invite.Do();
            invite.Commit();

            return invitationCode;
        }

        public async Task FinalizeInvitation(Guid invitationCode, string username, string password)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();
            FinalizeInvitation finalizeInvitation = new FinalizeInvitation(context, invitationCode, username, password);
            await finalizeInvitation.Do();
            await finalizeInvitation.CommitAsync();
        }
    }
}
