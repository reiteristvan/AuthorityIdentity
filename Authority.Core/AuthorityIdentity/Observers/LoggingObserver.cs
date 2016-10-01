using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.Observers
{
    public sealed class LoggingObserver : IAccountObserver
    {
        public void OnInvited(InviteInfo inviteInfo)
        {
            if (Authority.Logger == null)
            {
                return;
            }

            Authority.Logger.Info(string.Format(
                "User invited: {0} to {1}",
                inviteInfo.Email, inviteInfo.DomainId));
        }

        public void OnRegistering(RegistrationInfo registrationInfo)
        {
            if (Authority.Logger == null)
            {
                return;
            }

            Authority.Logger.Info(string.Format(
                "User registering: {0} to {1}", 
                registrationInfo.Email, registrationInfo.DomainId));
        }

        public void OnRegistered(User user)
        {
            if (Authority.Logger == null)
            {
                return;
            }

            Authority.Logger.Info(string.Format(
                "User registered: {0} to {1}",
                user.Email, user.DomainId));
        }

        public void OnActivated(User user)
        {
            if (Authority.Logger == null)
            {
                return;
            }

            Authority.Logger.Info(string.Format(
                "User activated: {0} to {1}",
                user.Email, user.DomainId));
        }

        public void OnLoggingIn(LoginInfo loginInfo)
        {
            if (Authority.Logger == null)
            {
                return;
            }

            Authority.Logger.Info(string.Format(
                "User logging in: {0} to {1}",
                loginInfo.Email, loginInfo.DomainId));
        }

        public void LoggedIn(User user)
        {
            if (Authority.Logger == null)
            {
                return;
            }

            Authority.Logger.Info(string.Format(
                "User logged in: {0} to {1}",
                user.Email, user.DomainId));
        }
    }
}
