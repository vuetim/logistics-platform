using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.DTOs.Orders
{
    public class ChangeOrderStatusDto
    {
        public OrderStatus NewStatus { get; set; }
        public string? Reason { get; set; }
    }
}
