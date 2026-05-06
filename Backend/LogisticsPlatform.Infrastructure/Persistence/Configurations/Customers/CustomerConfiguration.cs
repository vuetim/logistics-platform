using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Customers
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.OwnsOne(c => c.Billing, billing =>
            {
                billing.Property(b => b.Terms).HasConversion<int>();
                billing.Property(b => b.Method).HasConversion<int>();
                billing.Property(b => b.CreditLimit).HasPrecision(18, 2);
                billing.Property(b => b.AutoInvoice);
            });
        }
    }

    }
