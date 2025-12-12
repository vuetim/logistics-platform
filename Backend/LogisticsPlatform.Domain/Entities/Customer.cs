using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int PaymentTermsDays { get; set; } = 30; 

        public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();

        // public List<Load>? Loads { get; set; }
    }
}
