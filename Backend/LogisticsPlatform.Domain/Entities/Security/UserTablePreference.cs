using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities.Security
{
    public class UserTablePreference : BaseEntity
    {
        public Guid UserId { get; set; }
        public string TableKey { get; set; } = string.Empty;
        public string JsonConfig { get; set; } = "{}";
    }
}
