using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public interface ILoadCreationPolicy
{
    void ValidateDateOverrides(DateTime? plannedPickup, DateTime? plannedDelivery);
    void EnsureCanCreateFromOrder(Order order, Order orderWithLoads, CreateLoadFromOrderDto dto);
}

