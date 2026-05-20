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
            builder.Property(x => x.LoadNumber).HasMaxLength(80);
            builder.Property(x => x.CustomerRate).HasPrecision(18, 2);
            builder.Property(x => x.CarrierRate).HasPrecision(18, 2);
            builder.Property(x => x.Accessorials).HasPrecision(18, 2);
            builder.Property(x => x.DistanceMiles).HasPrecision(18, 2);
            builder.Property(x => x.LastKnownLatitude).HasPrecision(10, 7);
            builder.Property(x => x.LastKnownLongitude).HasPrecision(10, 7);
            builder.Property(x => x.EncodedPolyline).HasMaxLength(8000);
            builder.Property(x => x.TrackingProvider).HasMaxLength(80);
            builder.Property(x => x.TrackingExternalId).HasMaxLength(160);

            builder.HasIndex(x => x.LoadNumber);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.IsArchived);
            builder.HasIndex(x => x.CustomerId);
            builder.HasIndex(x => x.CarrierId);
            builder.HasIndex(x => x.Mode);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => new { x.CustomerId, x.Status });
            builder.HasIndex(x => new { x.CarrierId, x.Status });
            builder.HasIndex(x => new { x.IsArchived, x.Status });
            builder.HasIndex(x => new { x.IsArchived, x.CreatedAt });
            builder.HasIndex(x => x.TrackingExternalId);
        }
    }

}
