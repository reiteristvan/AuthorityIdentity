using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.EntityFramework
{
    public interface ISafeAuthorityContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Domain> Domains { get; set; }
        DbSet<AuthorityClaim> Claims { get; set; }
        DbSet<Policy> Policies { get; set; }
        DbSet<Invite> Invites { get; set; }
        DbSet<Group> Groups { get; set; }
        Database Database { get; }
        DbChangeTracker ChangeTracker { get; }
        DbContextConfiguration Configuration { get; }
        DbSet Set(Type entityType);
        DbEntityEntry Entry(object entity);
    }
}
