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
        }
    }

}
