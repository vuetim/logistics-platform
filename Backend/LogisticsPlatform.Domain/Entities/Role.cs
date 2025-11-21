using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public List<UserRole> Users { get; set; } = new();
}
