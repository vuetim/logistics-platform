using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Loads
{
    public class LoadNoteConfiguration : IEntityTypeConfiguration<LoadNote>
    {
        public void Configure(EntityTypeBuilder<LoadNote> b)
        {
            b.HasKey(x => x.Id);

            // Load -> Notes (Aggregate child)
            b.HasOne(n => n.Load)
                .WithMany(l => l.Notes)
                .HasForeignKey(n => n.LoadId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Notes (Audit reference ONLY)
            b.HasOne(n => n.CreatedByUser)
                .WithMany()
                .HasForeignKey(n => n.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); 

            b.HasIndex(x => x.LoadId);
        }
    }
}
