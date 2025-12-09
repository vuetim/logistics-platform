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
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderCost> OrderCosts => Set<OrderCost>();
    public DbSet<OrderExternalId> OrderExternalIds => Set<OrderExternalId>();
    public DbSet<OrderNote> OrderNotes => Set<OrderNote>();
    public DbSet<OrderDocument> OrderDocuments => Set<OrderDocument>();
    public DbSet<OrderEquipmentRequirement> OrderEquipmentRequirements => Set<OrderEquipmentRequirement>();
    public DbSet<OrderRoute> OrderRoutes => Set<OrderRoute>();

    public DbSet<ActivityLog> ActivityLogs { get; set; } = null!;
    public DbSet<LoadItem> LoadItems => Set<LoadItem>();

    public DbSet<OrderCostLineItem> OrderCostLineItems { get; set; } = null!;

    public DbSet<LoadCost> LoadCosts { get; set; } = null!;
    public DbSet<LoadCostLineItem> LoadCostLineItems { get; set; } = null!;


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
        // ORDER COST
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Cost)
            .WithOne(c => c.Order)
            .HasForeignKey<OrderCost>(c => c.OrderId);
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(x => x.CustomerRate)
                   .HasPrecision(18, 2);

        });


        modelBuilder.Entity<OrderCost>(entity =>
        {
            entity.Property(x => x.Notes)
                  .HasMaxLength(2000);

            entity.Property(x => x.TotalAmount)
                  .HasPrecision(18, 4);
        });

        modelBuilder.Entity<OrderCostLineItem>(entity =>
        {
          

            entity.Property(x => x.Qty)
                  .HasPrecision(18, 4);

            entity.Property(x => x.Price)
                  .HasPrecision(18, 4);

            entity.Property(x => x.Amount)
                  .HasPrecision(18, 4);
        });


        // LOAD COST
        modelBuilder.Entity<Load>()
            .HasOne(l => l.Cost)
            .WithOne(c => c.Load)
            .HasForeignKey<LoadCost>(c => c.LoadId);

        modelBuilder.Entity<LoadCost>(entity =>
        {
            entity.Property(x => x.Notes)
                  .HasMaxLength(2000);

            entity.Property(x => x.TotalAmount)
                  .HasPrecision(18, 4);
        });

        modelBuilder.Entity<LoadCostLineItem>(entity =>
        {


            entity.Property(x => x.Qty)
                  .HasPrecision(18, 4);

            entity.Property(x => x.Price)
                  .HasPrecision(18, 4);

            entity.Property(x => x.Amount)
                  .HasPrecision(18, 4);
        });
        modelBuilder.Entity<OrderCost>()
    .HasMany(c => c.LineItems)
    .WithOne(li => li.OrderCost)
    .HasForeignKey(li => li.OrderCostId)
    .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<LoadCost>()
    .HasMany(c => c.LineItems)
    .WithOne(li => li.LoadCost)
    .HasForeignKey(li => li.LoadCostId)
    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LoadOrder>(entity =>
        {
            entity.HasKey(lo => lo.Id);

            entity.HasOne(lo => lo.Order)
                .WithMany(o => o.Loads)
                .HasForeignKey(lo => lo.OrderId)
                .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(lo => lo.Load)
                .WithMany(l => l.Orders)
                .HasForeignKey(lo => lo.LoadId)
                .OnDelete(DeleteBehavior.Restrict); 
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(x => x.Quantity)
                .HasPrecision(18, 4);

            entity.Property(x => x.HandlingQuantity)
                .HasPrecision(18, 4);

            entity.Property(x => x.UnitNetWeight)
                .HasPrecision(18, 3);

            entity.Property(x => x.UnitGrossWeight)
                .HasPrecision(18, 3);

            entity.Property(x => x.Volume)
                .HasPrecision(18, 6);

            entity.Property(x => x.Length)
                .HasPrecision(18, 3);

            entity.Property(x => x.Width)
                .HasPrecision(18, 3);

            entity.Property(x => x.Height)
                .HasPrecision(18, 3);

            entity.Property(x => x.MinTemperature)
                .HasPrecision(5, 2);

            entity.Property(x => x.MaxTemperature)
                .HasPrecision(5, 2);

            entity.Property(x => x.DeclaredValue)
                .HasPrecision(18, 2);
        });
        modelBuilder.Entity<LoadStop>(entity =>
        {
            entity.Property(e => e.Latitude)
                .HasPrecision(9, 6);

            entity.Property(e => e.Longitude)
                .HasPrecision(9, 6);
        });


        modelBuilder.Entity<OrderEquipmentRequirement>(entity =>
        {
            entity.Property(x => x.MaxWeight)
                .HasPrecision(18, 3);

            entity.Property(x => x.RequiredTemperature)
                .HasPrecision(5, 2);
        });
        modelBuilder.Entity<OrderRoute>(entity =>
        {
            entity.Property(x => x.Latitude)
                .HasPrecision(9, 6);

            entity.Property(x => x.Longitude)
                .HasPrecision(9, 6);
        });

        modelBuilder.Entity<LoadItem>(entity =>
        {
            // Quantities
            entity.Property(x => x.Quantity)
                .HasPrecision(18, 4);

            entity.Property(x => x.HandlingQuantity)
                .HasPrecision(18, 4);

            // Weights
            entity.Property(x => x.UnitNetWeight)
                .HasPrecision(18, 4);

            entity.Property(x => x.UnitGrossWeight)
                .HasPrecision(18, 4);

            // Dimensions
            entity.Property(x => x.Length)
                .HasPrecision(18, 4);

            entity.Property(x => x.Width)
                .HasPrecision(18, 4);

            entity.Property(x => x.Height)
                .HasPrecision(18, 4);

            

            // Temperature
            entity.Property(x => x.MinTemperature)
                .HasPrecision(5, 2);

            entity.Property(x => x.MaxTemperature)
                .HasPrecision(5, 2);

            // Money
            entity.Property(x => x.DeclaredValue)
                .HasPrecision(18, 2);
        });


        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(x => x.Quantity)
                  .HasPrecision(18, 4);
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
  
        modelBuilder.Entity<OrderNote>()
            .HasOne(n => n.CreatedByUser)
            .WithMany()
            .HasForeignKey(n => n.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Load>().HasIndex(x => x.Status);
        modelBuilder.Entity<Load>().HasIndex(x => x.IsArchived);
        modelBuilder.Entity<Load>().HasIndex(x => new { x.CustomerId, x.Status });
        modelBuilder.Entity<Load>().HasIndex(x => new { x.IsArchived, x.Status });
        modelBuilder.Entity<CarrierAddress>()
    .HasIndex(x => new { x.CarrierId, x.IsPrimary });

        modelBuilder.Entity<CarrierAddress>()
            .HasIndex(x => new { x.CarrierId, x.IsActive });
        modelBuilder.Entity<CustomerAddress>()
            .HasIndex(x => new { x.CustomerId, x.IsPrimary });

        modelBuilder.Entity<CustomerAddress>()
            .HasIndex(x => new { x.CustomerId, x.IsActive });


    }
}
