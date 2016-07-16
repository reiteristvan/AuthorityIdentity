using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Authority.DomainModel;

namespace Authority.EntityFramework
{
    public interface ISafeAuthorityContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Domain> Domains { get; set; }
        DbSet<AuthorityClaim> Claims { get; set; }
        DbSet<Policy> Policies { get; set; }
        DbSet<Invite> Invites { get; set; }
        Database Database { get; }
        DbChangeTracker ChangeTracker { get; }
        DbContextConfiguration Configuration { get; }
        DbSet Set(Type entityType);
        DbEntityEntry Entry(object entity);
    }
}
