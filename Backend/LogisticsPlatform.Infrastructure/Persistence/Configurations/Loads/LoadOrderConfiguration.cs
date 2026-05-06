using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Loads
{
    public class LoadOrderConfiguration : IEntityTypeConfiguration<LoadOrder>
    {
        public void Configure(EntityTypeBuilder<LoadOrder> entity)
        {
            entity.HasKey(lo => lo.Id);

            entity.HasOne(lo => lo.Order)
                .WithMany(o => o.Loads)
                .HasForeignKey(lo => lo.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(lo => lo.Load)
                .WithMany(l => l.Orders)
                .HasForeignKey(lo => lo.LoadId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(lo => lo.OrderId);
            entity.HasIndex(lo => lo.LoadId);
        }
    }
}
