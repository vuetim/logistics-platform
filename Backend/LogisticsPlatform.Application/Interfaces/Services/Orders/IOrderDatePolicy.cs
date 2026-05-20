namespace LogisticsPlatform.Application.Interfaces.Services.Orders;

public interface IOrderDatePolicy
{
    void Validate(DateTime? start, DateTime? end, DateTime? plannedPickup, DateTime? plannedDelivery);
}

