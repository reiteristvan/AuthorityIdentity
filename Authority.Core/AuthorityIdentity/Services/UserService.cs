using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;
using AuthorityIdentity.Account;

namespace AuthorityIdentity.Services
{
    public interface IUserService
    {
        List<User> All(Guid domainId = new Guid());
        Task<User> FindByEmail(string email, Guid domainId = new Guid());
        Task<User> FindById(Guid id);
        IEnumerable<User> Find(Func<User, bool> predicate, bool includeDetails = false, Guid domainId = new Guid());
        Task<User> Register(string email, string username, string password, bool needToActivate = false, string metadata = "", Guid domainId = new Guid());
        Task Activate(Guid activationCode);
        Task<LoginResult> Login(string email, string password, Guid domainId = new Guid());
        Task Delete(Guid userId);
        Task SetStatus(Guid userId, bool isActive);
        Task BulkRegistration(List<BulkRegistrationData> registrationData, bool shouldActivate = false);
        Guid Invite(string email, DateTimeOffset? expireOn = null, Guid domainId = new Guid());
        Task FinalizeInvitation(Guid invitationCode, string username, string password);
        Task AddTwoFactorAuthentication(Guid userId, TwoFactorType type, string target);
        Task<bool> FinalizeTwoFactorAuthentication(Guid userId, string token);
        Task<string> GetMetadata(Guid userId);
        Task SetMetadata(Guid userId, string metadata);
    }

    public sealed class UserService : IUserService
    {
        /// <summary>
        /// List all the users within the domain identified by the domainId parameter. 
        /// Use with default values in Single domain environment.
        /// This call does not return detailed user entities.
        /// </summary>
        /// <param name="domainId">Id of the domain. With default value the function will use the first domain it find.</param>
        /// <returns>List of users</returns>
        public List<User> All(Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            IAuthorityContext context = AuthorityContextProvider.Create();

            List<User> users = context.Users.Where(u => u.DomainId == domainId).ToList();
            return users;
        }

        /// <summary>
        /// Find a user by email address within the domain identified by the domainId parameter.
        /// This call will return detailed user entities.
        /// Use default domainId value in Single Domain environment.
        /// </summary>
        /// <param name="email">Email address of a user</param>
        /// <param name="domainId">Id of a domain. With default value the function will use the first domain it find.</param>
        /// <returns>A detailed user entity.</returns>
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
                .Include(u => u.Groups.Select(g => g.Policies).Select(ps => ps.Select(p => p.Claims)))
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(p => p.Claims))
                .Include(u => u.Metadata)
                .FirstOrDefaultAsync(u => u.Email == email && u.DomainId == domainId);

            return user;
        }

        /// <summary>
        /// Find a user by its id. This call return detailed user entities.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>A detailed user entity.</returns>
        public async Task<User> FindById(Guid id)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            User user = await context.Users
                .Include(u => u.Groups)
                .Include(u => u.Groups.Select(g => g.Policies).Select(ps => ps.Select(p => p.Claims)))
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(p => p.Claims))
                .Include(u => u.Metadata)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        /// <summary>
        /// Find a user by a condition inside a given domain. Use default domainId value i na Single domain environment.
        /// </summary>
        /// <param name="predicate">Condition for finding a user.</param>
        /// <param name="includeDetails">Should the return include details.</param>
        /// <param name="domainId">Id of the domain where the search should happen.</param>
        /// <returns>A user entity.</returns>
        public IEnumerable<User> Find(Func<User, bool> predicate, bool includeDetails = false, Guid domainId = new Guid())
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            if (domainId == Guid.Empty)
            {
                domainId = Common.GetDomainId();
            }

            DbSet<User> users = context.Users;

            if (includeDetails)
            {
                users
                    .Include(u => u.Groups)
                    .Include(u => u.Groups.Select(g => g.Policies).Select(ps => ps.Select(p => p.Claims)))
                    .Include(u => u.Policies)
                    .Include(u => u.Policies.Select(p => p.Claims))
                    .Include(u => u.Metadata);
            }

            IEnumerable<User> result = users
                .Where(u => u.DomainId == domainId && predicate(u));

            return result;
        }

        public async Task<User> Register(string email, string username, string password, bool needToActivate = false, string metadata = "", Guid domainId = new Guid())
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
            RegisterUserModel model = new RegisterUserModel
            {
                DomainId    = domainId,
                Email = email,
                Username = username,
                Password = password,
                NeedToActivate = needToActivate,
                Metadata = metadata
            };

            RegisterUser registerOperation = new RegisterUser(context, model);
            User user = await registerOperation.Do();
            await registerOperation.CommitAsync();

            return user;
        }

        public async Task Activate(Guid activationCode)
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

        /// <summary>
        /// Enable two factor authentication for the user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="type">The type of 2FA (email, app, text, etc...)</param>
        /// <param name="target">Information how or where to send the 2FA code (phone number, email address, etc...)</param>
        /// <returns></returns>
        public async Task AddTwoFactorAuthentication(Guid userId, TwoFactorType type, string target)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            AddTwoFactorAuthenticationModel model = new AddTwoFactorAuthenticationModel
            {
                UserId = userId,
                TwoFactorType = type,
                TwoFactorTarget = target
            };

            AddTwoFactorAuthenticationToUser addTwoFactor = new AddTwoFactorAuthenticationToUser(context, model);
            await addTwoFactor.Do();
            await addTwoFactor.CommitAsync();
        }

        /// <summary>
        /// Finalize the login process with 2FA enabled. If it is failed the whole process should be started from the beginning.
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="token">2FA token sent to the user</param>
        /// <returns>2FA authentication was successfull or not</returns>
        public async Task<bool> FinalizeTwoFactorAuthentication(Guid userId, string token)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            FinalizeTwoFactorAuthentication finalize = new FinalizeTwoFactorAuthentication(context, userId, token);
            bool result = await finalize.Do();
            await finalize.CommitAsync();

            return result;
        }

        public async Task SetMetadata(Guid userId, string metadata)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            SetMetadata setMetadata = new SetMetadata(context, userId, metadata);
            await setMetadata.Do();
            await setMetadata.CommitAsync();
        }

        public async Task<string> GetMetadata(Guid userId)
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            Metadata metadata = await context.Metadata.FirstOrDefaultAsync(m => m.Id == userId);
            return metadata.Data;
        }
    }
}
