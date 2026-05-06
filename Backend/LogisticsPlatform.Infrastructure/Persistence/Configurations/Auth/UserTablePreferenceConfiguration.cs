using LogisticsPlatform.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Auth
{
    public class UserTablePreferenceConfiguration : IEntityTypeConfiguration<UserTablePreference>
    {
        public void Configure(EntityTypeBuilder<UserTablePreference> builder)
        {
            builder.ToTable("UserTablePreferences");

            builder.Property(x => x.TableKey)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(x => x.JsonConfig)
                .IsRequired();

            builder.HasIndex(x => new { x.UserId, x.TableKey })
                .IsUnique();
        }
    }
}
