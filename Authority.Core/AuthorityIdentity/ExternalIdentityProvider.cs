using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity
{
    public abstract class ExternalIdentityProvider
    {
        /// <summary>
        /// Name of the external provider which by Authority can find the registered Provider instance.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Creates the User instance from the external provider. 
        /// This function SHOULD set the ExternalToken property on the instance.
        /// </summary>
        /// <param name="tokenParameter">Parameter(s) which are neccesary for the registration process.</param>
        /// <returns>A User instance.</returns>
        public abstract Task<User> Register(object tokenParameter);

        /// <summary>
        /// Logins the User via the external provider. This function called internally.
        /// </summary>
        /// <param name="token">The value of the ExternalToken property of the User instance.</param>
        /// <returns>If the login process was successful or not.</returns>
        public abstract bool Login(string token);
    }
}
