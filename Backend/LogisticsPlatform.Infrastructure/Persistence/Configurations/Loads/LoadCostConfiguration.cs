using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Loads
{
    public class LoadCostConfiguration : IEntityTypeConfiguration<LoadCost>
    {
        public void Configure(EntityTypeBuilder<LoadCost> builder)
        {
            builder.Property(x => x.Notes).HasMaxLength(2000);
            builder.Property(x => x.TotalAmount).HasPrecision(18, 4);

            builder.HasMany(c => c.LineItems)
                .WithOne(li => li.LoadCost)
                .HasForeignKey(li => li.LoadCostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
