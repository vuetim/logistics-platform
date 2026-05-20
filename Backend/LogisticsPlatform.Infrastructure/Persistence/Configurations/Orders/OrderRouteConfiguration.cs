using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Orders;

public class OrderRouteConfiguration : IEntityTypeConfiguration<OrderRoute>
{
    public void Configure(EntityTypeBuilder<OrderRoute> builder)
    {
        builder.HasIndex(x => x.OrderId);
        builder.HasIndex(x => new { x.OrderId, x.IsActive, x.StopType, x.Sequence });
        builder.HasIndex(x => new { x.StopType, x.PlannedArrivalFrom });
        builder.HasIndex(x => new { x.StopType, x.PlannedArrivalTo });
    }
}

