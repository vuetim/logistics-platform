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
    public class LoadStopConfiguration : IEntityTypeConfiguration<LoadStop>
    {
        public void Configure(EntityTypeBuilder<LoadStop> builder)
        {
            builder.Property(e => e.Latitude).HasPrecision(9, 6);
            builder.Property(e => e.Longitude).HasPrecision(9, 6);
            builder.Property(e => e.PONumbers).HasMaxLength(500);
            builder.Property(e => e.TimeZone).HasMaxLength(64);
            builder.Property(e => e.AppointmentConfirmationNumber).HasMaxLength(120);

            builder.HasIndex(e => e.LoadId);
            builder.HasIndex(e => new { e.LoadId, e.StopType, e.Sequence });
            builder.HasIndex(e => new { e.StopType, e.PlannedArrivalFrom });
            builder.HasIndex(e => new { e.StopType, e.PlannedArrivalTo });
        }
    }

}
