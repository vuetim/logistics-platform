using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Carrier
{
    public class CarrierAddressConfiguration : IEntityTypeConfiguration<CarrierAddress>
    {
        public void Configure(EntityTypeBuilder<CarrierAddress> builder)
        {
            builder.HasIndex(x => new { x.CarrierId, x.IsPrimary });
            builder.HasIndex(x => new { x.CarrierId, x.IsActive });
        }
    }

}
