using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity
{
    public interface ITwoFactorService
    {
        /// <summary>
        /// Generates the token needed for Two Factor Authentication
        /// </summary>
        /// <returns></returns>
        string GenerateToken();

        /// <summary>
        /// Send the generated token to the user
        /// </summary>
        /// <param name="twoFactoryType">Type of the second factor (text, email, app, other)</param>
        /// <param name="target">How the user gets the token (phone number, email address, etc...)</param>
        /// <param name="token">The token</param>
        void SendToken(TwoFactorType twoFactoryType, string target, string token);
    }
}
