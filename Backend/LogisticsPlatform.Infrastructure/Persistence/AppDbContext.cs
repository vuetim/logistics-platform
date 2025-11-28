using LogisticsPlatform.Domain.Constants;
using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerContact> CustomerContacts => Set<CustomerContact>();
    public DbSet<CustomerNote> CustomerNotes => Set<CustomerNote>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<CarrierContact> CarrierContacts => Set<CarrierContact>();
    public DbSet<CarrierAddress> CarrierAddresses => Set<CarrierAddress>();
    public DbSet<CarrierNote> CarrierNotes => Set<CarrierNote>();
    public DbSet<CarrierDocument> CarrierDocuments => Set<CarrierDocument>();

    public DbSet<Load> Loads => Set<Load>();
    public DbSet<LoadStop> LoadStops => Set<LoadStop>();
    public DbSet<LoadEquipment> LoadEquipment => Set<LoadEquipment>();
    public DbSet<LoadOrder> LoadOrders => Set<LoadOrder>();
    public DbSet<LoadNote> LoadNotes => Set<LoadNote>();
    public DbSet<LoadDocument> LoadDocuments => Set<LoadDocument>();


    public DbSet<Carrier> Carriers => Set<Carrier>();





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)   // ✔ navigation property in User
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)   // ✔ navigation property in Role
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<Role>().HasData(
           new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), Name = RoleNames.Admin },
           new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), Name = RoleNames.Broker },
           new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), Name = RoleNames.Operator },
           new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), Name = RoleNames.Dispatcher },
           new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), Name = RoleNames.Accounting }
       );
        modelBuilder.Entity<LoadEquipment>(entity =>
        {
            entity.Property(x => x.Weight)
                  .HasPrecision(10, 2);

        

            entity.Property(x => x.Length)
                  .HasPrecision(10, 2);

            entity.Property(x => x.MinTemp)
                  .HasPrecision(5, 2);

            entity.Property(x => x.MaxTemp)
                  .HasPrecision(5, 2);
        });
        modelBuilder.Entity<Load>(entity =>
        {
            entity.Property(x => x.CustomerRate)
                  .HasPrecision(18, 2);

            entity.Property(x => x.CarrierRate)
                  .HasPrecision(18, 2);

            entity.Property(x => x.Accessorials)
                  .HasPrecision(18, 2);
        });
        modelBuilder.Entity<LoadNote>(b =>
        {
            b.HasOne(n => n.Load)
                .WithMany(l => l.Notes)
                .HasForeignKey(n => n.LoadId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(n => n.CreatedByUser)
                .WithMany()
                .HasForeignKey(n => n.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
        });

    }
}
