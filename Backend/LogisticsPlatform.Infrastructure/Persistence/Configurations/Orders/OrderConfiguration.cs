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

            builder.Property(x => x.CustomerRate).HasPrecision(18, 2);
            builder.Property(x => x.TotalVolume).HasPrecision(18, 4);
            builder.Property(x => x.TotalWeight).HasPrecision(18, 2);
        }
    }

}
