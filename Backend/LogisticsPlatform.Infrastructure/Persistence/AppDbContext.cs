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



    public DbSet<Carrier> Carriers => Set<Carrier>();





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}
