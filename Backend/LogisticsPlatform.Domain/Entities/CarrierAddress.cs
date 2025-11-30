using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Domain.Entities
{
    public class CarrierAddress : BaseEntity
    {
        public Guid CarrierId { get; set; }
        public Carrier Carrier { get; set; } = null!;

        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public CarrierAddressType Type { get; set; } = CarrierAddressType.General;
        public bool IsPrimary { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
