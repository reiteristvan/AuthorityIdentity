using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        List<Domain> All(bool forceReload = false);
        Task<Domain> FindById(Guid domainId, bool includePolicyDetails = false);
        Task<Guid> Create(string name);
        Task Delete(Guid domainId);
    }

    internal interface IInternalDomainService
    {
        void LoadDomains(bool forceReload);
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

        public List<Domain> All(bool forceReload = false)
        {
            ((IInternalDomainService) this).LoadDomains(forceReload);
            return Authority.DomainMode == DomainMode.Multi ?
                _domains : 
                _domains.Where(d => d.Name.Equals(MasterDomainName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public async Task<Domain> FindById(Guid domainId, bool includePolicyDetails = false)
        {
            using (AuthorityContext context = new AuthorityContext())
            {
                IQueryable<Domain> query = context.Domains
                    .Include(d => d.Groups)
                    .Include(d => d.Claims)
                    .Include(d => d.Policies);

                if (includePolicyDetails)
                {
                    query = query.Include(d => d.Policies.Select(p => p.Claims));
                }

                Domain domain = await query.FirstOrDefaultAsync(d => d.Id == domainId);

                return domain;
            }
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

        public async Task Delete(Guid domainId)
        {
            using (AuthorityContext context = AuthorityContextProvider.Create())
            {
                DeleteDomain delete = new DeleteDomain(context, domainId);
                await delete.Execute();

                _changed = true;
            }
        }

        void IInternalDomainService.LoadDomains(bool forceReload)
        {
            lock (_domainLock)
            {
                if (_changed.HasValue && !_changed.Value && !forceReload)
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
