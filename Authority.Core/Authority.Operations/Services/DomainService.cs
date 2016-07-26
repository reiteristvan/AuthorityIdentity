using System.Collections.Generic;
using System.Linq;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Services
{
    public interface IDomainService
    {
        List<Domain> All();
    }

    internal interface IInternalDomainService
    {
        void LoadDomains();
    }

    public sealed class DomainService : IDomainService, IInternalDomainService
    {
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
            return _domains;
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
