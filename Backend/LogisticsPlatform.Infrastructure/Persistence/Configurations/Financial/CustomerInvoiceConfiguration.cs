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
    public class CustomerInvoiceConfiguration : IEntityTypeConfiguration<CustomerInvoice>
    {
        public void Configure(EntityTypeBuilder<CustomerInvoice> builder)
        {
            builder.Property(x => x.TotalAmount).HasPrecision(18, 2);

            builder.HasOne(x => x.Load)
                .WithMany()
                .HasForeignKey(x => x.LoadId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.LineItems)
                .WithOne(li => li.Invoice)
                .HasForeignKey(li => li.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
