using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Orders
{
    public class OrderCostConfiguration : IEntityTypeConfiguration<OrderCost>
    {
        public void Configure(EntityTypeBuilder<OrderCost> builder)
        {
            builder.Property(x => x.BillTo).HasMaxLength(200);
            builder.Property(x => x.Notes).HasMaxLength(2000);
            builder.Property(x => x.TaxRate).HasPrecision(8, 4);
            builder.Property(x => x.TotalAmount).HasPrecision(18, 4);

            builder.HasMany(c => c.LineItems)
                .WithOne(li => li.OrderCost)
                .HasForeignKey(li => li.OrderCostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
