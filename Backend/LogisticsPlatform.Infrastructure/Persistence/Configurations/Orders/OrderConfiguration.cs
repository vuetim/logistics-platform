using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.Cost)
                .WithOne(c => c.Order)
                .HasForeignKey<OrderCost>(c => c.OrderId);

            builder.Property(x => x.OrderNumber).HasMaxLength(80);
            builder.Property(x => x.CustomerRate).HasPrecision(18, 2);
            builder.Property(x => x.TotalVolume).HasPrecision(18, 4);
            builder.Property(x => x.TotalWeight).HasPrecision(18, 2);

            builder.HasIndex(x => x.OrderNumber);
            builder.HasIndex(x => x.CustomerId);
            builder.HasIndex(x => x.PreferredCarrierId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Phase);
            builder.HasIndex(x => x.Direction);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.StartDate);
            builder.HasIndex(x => x.EndDate);
            builder.HasIndex(x => x.PlannedPickupDate);
            builder.HasIndex(x => x.PlannedDeliveryDate);
            builder.HasIndex(x => new { x.CustomerId, x.Status });
            builder.HasIndex(x => new { x.Status, x.CreatedAt });
        }
    }

}
