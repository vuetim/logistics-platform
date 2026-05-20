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
    public class LoadCarrierAssignmentConfiguration : IEntityTypeConfiguration<LoadCarrierAssignment>
    {
        public void Configure(EntityTypeBuilder<LoadCarrierAssignment> builder)
        {
            builder.HasOne(x => x.Load)
                .WithMany(l => l.CarrierAssignments)
                .HasForeignKey(x => x.LoadId);

            builder.HasOne(x => x.Carrier)
                .WithMany()
                .HasForeignKey(x => x.CarrierId);

            builder.Property(x => x.OfferedRate).HasPrecision(12, 2);
            builder.Property(x => x.Currency).HasMaxLength(10);
            builder.Property(x => x.RateConfirmationNumber).HasMaxLength(80);
            builder.Property(x => x.TenderMethod).HasMaxLength(40);
            builder.Property(x => x.TenderToken).HasMaxLength(120);
            builder.Property(x => x.TenderEmailTo).HasMaxLength(200);
            builder.Property(x => x.AcceptedByName).HasMaxLength(160);
            builder.Property(x => x.AcceptedByEmail).HasMaxLength(200);
            builder.Property(x => x.AcceptedByPhone).HasMaxLength(40);

            builder.HasIndex(x => x.LoadId);
            builder.HasIndex(x => x.CarrierId);
            builder.HasIndex(x => new { x.LoadId, x.IsActive });
            builder.HasIndex(x => new { x.CarrierId, x.Status });
            builder.HasIndex(x => x.TenderExpiresAt);
            builder.HasIndex(x => x.TenderToken).IsUnique().HasFilter("[TenderToken] IS NOT NULL");
        }
    }

}
