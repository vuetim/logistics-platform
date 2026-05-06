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
    public class LoadEquipmentConfiguration : IEntityTypeConfiguration<LoadEquipment>
    {
        public void Configure(EntityTypeBuilder<LoadEquipment> builder)
        {
            builder.Property(x => x.Weight).HasPrecision(10, 2);
            builder.Property(x => x.Length).HasPrecision(10, 2);
            builder.Property(x => x.MinTemp).HasPrecision(5, 2);
            builder.Property(x => x.MaxTemp).HasPrecision(5, 2);
        }
    }

}
