using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.EntityFramework.Configurations;

namespace Authority.EntityFramework
{
    public sealed class AuthorityContext : DbContext, IAuthorityContext
    {
        private DbContextTransaction _transaction;

        public AuthorityContext()
            : base("name=AuthorityConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Policy> Policies { get; set; } 
        public DbSet<AuthorityClaim> Claims { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new DomainConfiguration());
            modelBuilder.Configurations.Add(new AuthorityClaimConfiguration());
            modelBuilder.Configurations.Add(new PolicyConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                IEnumerable<string> errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                string fullErrorMessage = string.Join("; ", errorMessages);
                string exceptionMessage = string.Concat(ex.Message, "The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                IEnumerable<string> errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                string fullErrorMessage = string.Join("; ", errorMessages);
                string exceptionMessage = string.Concat(ex.Message, "The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _transaction = Database.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
        }

        public new void Dispose()
        {
            if (_transaction != null && 
                (_transaction.UnderlyingTransaction.Connection == null || _transaction.UnderlyingTransaction.Connection.State != ConnectionState.Closed))
            {
                _transaction.Dispose();
            }

            base.Dispose();
        }
    }
}
