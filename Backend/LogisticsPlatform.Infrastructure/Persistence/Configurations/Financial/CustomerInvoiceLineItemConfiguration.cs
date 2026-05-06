using LogisticsPlatform.Domain.Entities.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Financial
{
    public class CustomerInvoiceLineItemConfiguration : IEntityTypeConfiguration<CustomerInvoiceLineItem>
    {
        public void Configure(EntityTypeBuilder<CustomerInvoiceLineItem> builder)
        {
            builder.Property(x => x.Qty).HasPrecision(18, 2);
            builder.Property(x => x.Price).HasPrecision(18, 2);
            builder.Property(x => x.Amount).HasPrecision(18, 2);
        }
    }

}
