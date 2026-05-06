using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Loads
{
    public class LoadItemConfiguration : IEntityTypeConfiguration<LoadItem>
    {
        public void Configure(EntityTypeBuilder<LoadItem> builder)
        {
            builder.Property(x => x.Quantity).HasPrecision(18, 4);
            builder.Property(x => x.HandlingQuantity).HasPrecision(18, 4);
            builder.Property(x => x.UnitNetWeight).HasPrecision(18, 4);
            builder.Property(x => x.UnitGrossWeight).HasPrecision(18, 4);
            builder.Property(x => x.Length).HasPrecision(18, 4);
            builder.Property(x => x.Width).HasPrecision(18, 4);
            builder.Property(x => x.Height).HasPrecision(18, 4);
            builder.Property(x => x.Volume).HasPrecision(18, 4);
            builder.Property(x => x.MinTemperature).HasPrecision(5, 2);
            builder.Property(x => x.MaxTemperature).HasPrecision(5, 2);
            builder.Property(x => x.DeclaredValue).HasPrecision(18, 2);
        }
    }

}
