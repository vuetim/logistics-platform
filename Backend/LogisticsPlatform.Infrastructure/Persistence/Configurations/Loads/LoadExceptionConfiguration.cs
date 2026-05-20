using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Loads;

public class LoadExceptionConfiguration : IEntityTypeConfiguration<LoadException>
{
    public void Configure(EntityTypeBuilder<LoadException> builder)
    {
        builder.Property(x => x.ExceptionKey).HasMaxLength(80);
        builder.Property(x => x.ExceptionValue).HasMaxLength(160);
        builder.Property(x => x.ReasonKey).HasMaxLength(80);
        builder.Property(x => x.ReasonValue).HasMaxLength(160);
        builder.Property(x => x.EdiReasonCode).HasMaxLength(20);
        builder.Property(x => x.ResponsiblePartyKey).HasMaxLength(80);
        builder.Property(x => x.ResponsiblePartyValue).HasMaxLength(160);
        builder.Property(x => x.Unit).HasMaxLength(40);
        builder.Property(x => x.AffectedItemName).HasMaxLength(200);
        builder.Property(x => x.AffectedItemReference).HasMaxLength(120);

        builder.HasOne(x => x.Load)
            .WithMany(x => x.Exceptions)
            .HasForeignKey(x => x.LoadId);

        builder.HasOne(x => x.LoadStop)
            .WithMany()
            .HasForeignKey(x => x.LoadStopId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.LoadId, x.Status });
    }
}
