using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;
using Authority.Operations.Configuration;
using Authority.Operations.Products;

namespace Authority.Operations.Services
{
    public interface IDomainService
    {
        List<Domain> All();
        Task<Guid> Create(string name);
    }

    internal interface IInternalDomainService
    {
        void LoadDomains();
    }

    public sealed class DomainService : IDomainService, IInternalDomainService
    {
        public const string MasterDomainName = "master";

        private readonly List<Domain> _domains;
        private readonly object _domainLock;
        private bool? _changed;

        public DomainService()
        {
            _domains = new List<Domain>();
            _domainLock = new object();
            _changed = null;
        }

        public List<Domain> All()
        {
            ((IInternalDomainService) this).LoadDomains();
            return Authority.Configuration.DomainMode == DomainMode.Multi ?
                _domains : 
                _domains.Where(d => d.Name.Equals(MasterDomainName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public async Task<Guid> Create(string name)
        {
            using (AuthorityContext context = AuthorityContextProvider.Create())
            {
                CreateDomain create = new CreateDomain(context, name);
                Guid domainId = await create.Do();
                await create.CommitAsync();

                _changed = true;
                return domainId;
            }
        }

        void IInternalDomainService.LoadDomains()
        {
            lock (_domainLock)
            {
                if (_changed.HasValue && !_changed.Value)
                {
                    return;
                }

                IAuthorityContext context = AuthorityContextProvider.Create();
                List<Domain> domains = context.Domains.ToList();

                _domains.Clear();
                _domains.AddRange(domains);
                _changed = false;
            }
        }
    }
}
