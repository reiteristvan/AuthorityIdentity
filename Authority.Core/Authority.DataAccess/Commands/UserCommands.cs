using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.Models.UserModels;

namespace Authority.DataAccess.Commands
{
    public interface IUserCommands
    {
        void Create(User user);
        void Delete(Guid userId);
        void SetStatus(Guid userId, bool isActive);
        void Activate(Guid userId);
    }

    public sealed class UserCommands : IUserCommands
    {
        public void Create(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void SetStatus(Guid userId, bool isActive)
        {
            throw new NotImplementedException();
        }

        public void Activate(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
