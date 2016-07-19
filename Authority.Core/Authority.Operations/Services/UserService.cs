using System;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Account;

namespace Authority.Operations.Services
{
    public interface IUserService
    {
        Task<User> Register(string email, string username, string password, bool needToActivate = false, Guid domainId = new Guid());
        Task Acivate(Guid activationCode);
        Task<LoginResult> Login(string email, string password, Guid domainId = new Guid());
        Task Delete(Guid domainId, string email);
    }

    public sealed class UserService : IUserService
    {
        private readonly IAuthorityContext _context;

        public UserService()
        {
            _context = new AuthorityContext();
        }

        public async Task<User> Register(string email, string username, string password, bool needToActivate = false, Guid domainId = new Guid())
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("One or more argument is invalid");
            }

            // it is single domain mode OR the user's domain will be the first one
            if (domainId == Guid.Empty)
            {
                domainId = GetDomainId();
            }

            RegisterUser registerOperation = new RegisterUser(_context, domainId, email, username, password, needToActivate);
            User user = await registerOperation.Do();
            await registerOperation.CommitAsync();

            return user;
        }

        public async Task Acivate(Guid activationCode)
        {
            ActivateUser activateOperation = new ActivateUser(_context, activationCode);
            await activateOperation.Do();
            await activateOperation.CommitAsync();
        }

        public async Task<LoginResult> Login(string email, string password, Guid domainId = new Guid())
        {
            if (domainId == Guid.Empty)
            {
                domainId = GetDomainId();
            }

            LoginUser loginOperation = new LoginUser(_context, domainId, email, password);
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

            DeleteUser deleteOperation = new DeleteUser(_context, domainId, email);
            await deleteOperation.Do();
            await deleteOperation.CommitAsync();
        }

        private Guid GetDomainId()
        {
            Domain domain = _context.Domains.FirstOrDefault();

            if (domain == null)
            {
                throw new InvalidOperationException("No domain exists");
            }

            return domain.Id;
        }
    }
}
