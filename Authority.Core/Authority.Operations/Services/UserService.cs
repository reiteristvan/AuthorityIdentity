using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Account;

namespace Authority.Operations.Services
{
    public interface IUserService
    {
        Task<User> FindByEmail(string email, Guid domainId = new Guid());
        Task<User> FindById(Guid id, Guid domainId = new Guid());
        Task<User> Register(string email, string username, string password, bool needToActivate = false, Guid domainId = new Guid());
        Task Acivate(Guid activationCode);
        Task<LoginResult> Login(string email, string password, Guid domainId = new Guid());
        Task Delete(Guid domainId, string email);
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
                domainId = GetDomainId(context);
            }

            User user = await context.Users
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(p => p.Claims))
                .FirstOrDefaultAsync(u => u.Email == email && u.DomainId == domainId);

            return user;
        }

        public async Task<User> FindById(Guid id, Guid domainId = new Guid())
        {
            IAuthorityContext context = AuthorityContextProvider.Create();

            if (domainId == Guid.Empty)
            {
                domainId = GetDomainId(context);
            }

            User user = await context.Users
                .Include(u => u.Policies)
                .Include(u => u.Policies.Select(p => p.Claims))
                .FirstOrDefaultAsync(u => u.Id == id && u.DomainId == domainId);

            return user;
        }

        public async Task<User> Register(string email, string username, string password, bool needToActivate = false, Guid domainId = new Guid())
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("One or more argument is invalid");
            }

            IAuthorityContext context = AuthorityContextProvider.Create();

            // it is single domain mode OR the user's domain will be the first one
            if (domainId == Guid.Empty)
            {
                domainId = GetDomainId(context);
            }

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
            IAuthorityContext context = AuthorityContextProvider.Create();

            if (domainId == Guid.Empty)
            {
                domainId = GetDomainId(context);
            }

            LoginUser loginOperation = new LoginUser(context, domainId, email, password);
            LoginResult result = await loginOperation.Do();
            await loginOperation.CommitAsync();

            return result;
        }

        public async Task Delete(Guid domainId, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Invalid email");
            }

            IAuthorityContext context = AuthorityContextProvider.Create();
            DeleteUser deleteOperation = new DeleteUser(context, domainId, email);
            await deleteOperation.Do();
            await deleteOperation.CommitAsync();
        }

        public async Task SetStatus(Guid id)
        {
            
        }

        private Guid GetDomainId(IAuthorityContext context)
        {
            Domain domain = context.Domains.FirstOrDefault();

            if (domain == null)
            {
                throw new InvalidOperationException("No domain exists");
            }

            return domain.Id;
        }
    }
}
