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
        }
    }

}
