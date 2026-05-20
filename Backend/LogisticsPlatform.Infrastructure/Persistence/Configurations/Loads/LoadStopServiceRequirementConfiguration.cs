using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Loads;

public class LoadStopServiceRequirementConfiguration : IEntityTypeConfiguration<LoadStopServiceRequirement>
{
    public void Configure(EntityTypeBuilder<LoadStopServiceRequirement> builder)
    {
        builder.Property(x => x.ServiceKey).HasMaxLength(80);
        builder.Property(x => x.ServiceValue).HasMaxLength(160);

        builder.HasOne(x => x.LoadStop)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.LoadStopId);

        builder.HasIndex(x => new { x.LoadStopId, x.ServiceKey });
    }
}
