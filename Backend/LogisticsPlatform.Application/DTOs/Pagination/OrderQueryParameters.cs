using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Orders
{
    public class OrderQueryParameters : QueryParameters
    {
        public Guid? CustomerId { get; set; }
        public Guid? PreferredCarrierId { get; set; }

        public OrderStatus? Status { get; set; }
        public OrderPhase? Phase { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
