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
    public class LoadConfiguration : IEntityTypeConfiguration<Load>
    {
        public void Configure(EntityTypeBuilder<Load> builder)
        {
            builder.Property(x => x.CustomerRate).HasPrecision(18, 2);
            builder.Property(x => x.CarrierRate).HasPrecision(18, 2);
            builder.Property(x => x.Accessorials).HasPrecision(18, 2);

            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.IsArchived);
            builder.HasIndex(x => new { x.CustomerId, x.Status });
            builder.HasIndex(x => new { x.IsArchived, x.Status });
        }
    }

}
