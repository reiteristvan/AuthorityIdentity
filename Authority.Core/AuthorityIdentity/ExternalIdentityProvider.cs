using System.Threading.Tasks;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity
{
    public abstract class ExternalIdentityProvider
    {
        /// <summary>
        /// Name of the external provider which by Authority can find the registered Provider instance.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Creates the User instance from the external provider. 
        /// This function SHOULD set the ExternalToken, Email andUsername properties on the instance.
        /// Metadata property is optional, if not set internally set to empty
        /// </summary>
        /// <param name="tokenParameter">Parameter(s) which are neccesary for the registration process.</param>
        /// <returns>A User instance.</returns>
        public abstract Task<User> Register(object tokenParameter);
    }
}
