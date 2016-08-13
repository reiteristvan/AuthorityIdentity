using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Groups
{
    public sealed class CreateGroup : OperationWithReturnValueAsync<Group>
    {
        private readonly Guid _domainId;
        private readonly string _name;
        private readonly bool _defaultGroup;
        private readonly bool _replaceDefault;
        private readonly IList<User> _users;

        public CreateGroup(
            IAuthorityContext authorityContext, 
            Guid domainId,
            string name, 
            bool defaultGroup = false, 
            bool replaceDefault = false,
            IList<User> users = null) 
            : base(authorityContext)
        {
            _domainId = domainId;
            _name = name;
            _defaultGroup = defaultGroup;
            _replaceDefault = replaceDefault;
            _users = users;
        }

        public override async Task<Group> Do()
        {
            Domain domain = await Context.Domains
                .FirstOrDefaultAsync(d => d.Id == _domainId);

            Require(() => domain != null, ErrorCodes.DomainNotFound);

            Group existing = await Context.Groups.FirstOrDefaultAsync(g => g.Name == _name && g.DomainId == domain.Id);

            Require(() => existing == null, ErrorCodes.GroupNameNotAvailable);

            Group group = new Group
            {
                DomainId = domain.Id,
                IsActive = true,
                Name = _name,
                Default = _defaultGroup
            };

            if (_defaultGroup)
            {
                Group defaultGroup = await Context.Groups.FirstOrDefaultAsync(g => g.Default);

                if (defaultGroup != null)
                {
                    if (!_replaceDefault)
                    {
                        throw new RequirementFailedException(ErrorCodes.DefaultGroupAlreadyExists);
                    }

                    defaultGroup.Default = false;
                }
            }

            Context.Groups.Add(group);

            if (_users != null)
            {
                foreach (User user in _users)
                {
                    if (user.DomainId != domain.Id)
                    {
                        throw new RequirementFailedException(ErrorCodes.DomainNotFound);
                    }

                    group.Users.Add(user);
                }
            }

            return group;
        }
    }
}
