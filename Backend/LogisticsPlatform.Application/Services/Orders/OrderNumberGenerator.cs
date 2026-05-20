using LogisticsPlatform.Application.Interfaces.Services.Orders;

namespace LogisticsPlatform.Application.Services.Orders;

public class OrderNumberGenerator : IOrderNumberGenerator
{
    public string Generate()
        => $"O-{DateTime.UtcNow:yyyyMMddHHmmss}";
}

