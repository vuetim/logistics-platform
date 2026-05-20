using LogisticsPlatform.Domain.Constants;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Entities.Financial;
using LogisticsPlatform.Domain.Entities.Security;
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
    //activitylog
    public DbSet<ActivityLog> ActivityLogs { get; set; } = null!;
    //loaditems
    public DbSet<LoadItem> LoadItems => Set<LoadItem>();
    //costs
    public DbSet<OrderCostLineItem> OrderCostLineItems { get; set; } = null!;

    public DbSet<LoadCost> LoadCosts { get; set; } = null!;
    public DbSet<LoadCostLineItem> LoadCostLineItems { get; set; } = null!;
    //financials
    public DbSet<CustomerInvoice> CustomerInvoices => Set<CustomerInvoice>();
    public DbSet<CustomerInvoiceLineItem> CustomerInvoiceLineItems => Set<CustomerInvoiceLineItem>();

    public DbSet<CarrierSettlement> CarrierSettlements => Set<CarrierSettlement>();
    public DbSet<CarrierSettlementLineItem> CarrierSettlementLineItems => Set<CarrierSettlementLineItem>();
    //carrierassignment
    public DbSet<LoadCarrierAssignment> LoadCarrierAssignments { get; set; }
    //carrier performance 

    public DbSet<CarrierStopPerformance> CarrierStopPerformances { get; set; }
    public DbSet<LoadAlert> LoadAlerts => Set<LoadAlert>();
    public DbSet<LoadException> LoadExceptions => Set<LoadException>();
    public DbSet<LoadStopServiceRequirement> LoadStopServiceRequirements => Set<LoadStopServiceRequirement>();

    public DbSet<DelayResponsibility> DelayResponsibilities { get; set; }
    public DbSet<LoadDelayResponsibility> LoadDelayResponsibilities { get; set; }
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<AuthAuditLog> AuthAuditLogs => Set<AuthAuditLog>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserTablePreference> UserTablePreferences => Set<UserTablePreference>();
    public DbSet<UserNotification> UserNotifications => Set<UserNotification>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // 🔥 GLOBAL DECIMAL RULE (EF Core 8 compatible)
        modelBuilder
            .Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))
            .ToList()
            .ForEach(p =>
            {
                p.SetPrecision(18);
                p.SetScale(4);
            });
    }
}
