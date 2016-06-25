using Authority.DomainModel;
using Serilog.Events;

namespace Authority.Operations.Observers
{
    public sealed class LoggingObserver : IAccountObserver
    {
        public void OnRegistering(RegistrationInfo registrationInfo)
        {
            Authority.Logger.Write(
                LogEventLevel.Information, 
                "User registering: {0} to {1}", 
                registrationInfo.Email, registrationInfo.ProductId);
        }

        public void OnRegistered(User user)
        {
            Authority.Logger.Write(
                LogEventLevel.Information,
                "User registered: {0} to {1}",
                user.Email, user.DomainId);
        }

        public void OnActivated(User user)
        {
            Authority.Logger.Write(
                LogEventLevel.Information,
                "User activated: {0} to {1}",
                user.Email, user.DomainId);
        }

        public void OnLoggingIn(LoginInfo loginInfo)
        {
            Authority.Logger.Write(
                LogEventLevel.Information,
                "User logging in: {0} to {1}",
                loginInfo.Email, loginInfo.ProductId);
        }

        public void LoggedIn(User user)
        {
            Authority.Logger.Write(
                LogEventLevel.Information,
                "User logged in: {0} to {1}",
                user.Email, user.DomainId);
        }
    }
}
