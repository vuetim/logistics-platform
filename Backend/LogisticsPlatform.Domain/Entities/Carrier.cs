using LogisticsPlatform.Domain.Common;

namespace LogisticsPlatform.Domain.Entities
{
    public class Carrier : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string McNumber { get; set; } = string.Empty;   
        public string DotNumber { get; set; } = string.Empty;  

        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Active, Inactive, Blocked
        public string Status { get; set; } = "Active";

        // 1–5 rating (opsionale për tani)
        public int Rating { get; set; } = 0;

        public ICollection<CarrierContact> Contacts { get; set; } = new List<CarrierContact>();
        // public ICollection<CarrierNote> Notes { get; set; } = new List<CarrierNote>();
    }
}
