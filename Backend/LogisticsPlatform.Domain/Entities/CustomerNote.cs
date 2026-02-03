using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class CustomerNote : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
    }
}
