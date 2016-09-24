using System;
using System.Threading.Tasks;

namespace Authority
{
    public interface IAuthorityEmailService
    {
        void SendUserActivationEmail(string email, Guid activationCode);
        Task SendUserActivationEmailAsync(string email, Guid activationCode);
        void SendInvite(Guid invitationCode);
        Task SendInviteAsync(Guid invitationCode);
        void SendWelcomeEmail(string email);
        Task SendWelcomeEmailAsync(string email);
    }
}
