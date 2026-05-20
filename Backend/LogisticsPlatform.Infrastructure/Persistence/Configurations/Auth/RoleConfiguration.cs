using LogisticsPlatform.Domain.Constants;
using LogisticsPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Infrastructure.Persistence.Configurations.Auth
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        private static readonly DateTime SeedCreatedAt = new(2025, 11, 26, 16, 47, 48, DateTimeKind.Utc);

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), Name = RoleNames.Admin, CreatedAt = SeedCreatedAt },
                new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), Name = RoleNames.Broker, CreatedAt = SeedCreatedAt },
                new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), Name = RoleNames.Operator, CreatedAt = SeedCreatedAt },
                new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), Name = RoleNames.Dispatcher, CreatedAt = SeedCreatedAt },
                new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), Name = RoleNames.Accounting, CreatedAt = SeedCreatedAt },
                new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6"), Name = RoleNames.Sales, CreatedAt = SeedCreatedAt }
            );
        }
    }


}
