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
    public class LoadCostLineItemConfiguration : IEntityTypeConfiguration<LoadCostLineItem>
    {
        public void Configure(EntityTypeBuilder<LoadCostLineItem> builder)
        {
            builder.Property(x => x.Qty).HasPrecision(18, 4);
            builder.Property(x => x.Price).HasPrecision(18, 4);
            builder.Property(x => x.Amount).HasPrecision(18, 4);
        }
    }

}
