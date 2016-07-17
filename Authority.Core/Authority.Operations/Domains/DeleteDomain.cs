using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Products
{
    public sealed class DeleteDomain : SqlOperation
    {
        private readonly Guid _domainId;

        public DeleteDomain(IAuthorityContext AuthorityContext, Guid domainId)
            : base(AuthorityContext)
        {
            _domainId = domainId;
        }

        protected override async Task Do()
        {
            Domain domain = await Context.Domains
                .FirstOrDefaultAsync(d => d.Id == _domainId);
            
            if (domain == null)
            {
                throw new RequirementFailedException(DomainErrorCodes.DomainNotExists);
            }

            await Context.Database.ExecuteSqlCommandAsync(
                "delete from Authority.Users where DomainId = @DomainId",
                new SqlParameter("@DomainId", _domainId));

            await Context.Database.ExecuteSqlCommandAsync(
                "delete from Authority.Domains where Id = @Id",
                new SqlParameter("@Id", _domainId));
        }
    }
}
