using LogisticsPlatform.Application.DTOs.Orders;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Orders;

public interface IOrderDetailsMapper
{
    OrderDetailsDto Map(Order order);
}

