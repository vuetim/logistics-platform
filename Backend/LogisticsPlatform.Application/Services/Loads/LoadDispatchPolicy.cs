using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services.Loads;

public class LoadDispatchPolicy : ILoadDispatchPolicy
{
    public void EnsureCanDispatch(Load load, DispatchLoadDto dto)
    {
        if (load.Status != LoadStatus.Accepted)
            throw new BusinessRuleException("Only accepted loads can be dispatched.");

        if (load.CarrierId == null)
            throw new BusinessRuleException("Carrier must be assigned before dispatch.");

        if (string.IsNullOrWhiteSpace(dto.DriverName))
            throw new BusinessRuleException("Driver name is required before dispatch.");

        if (string.IsNullOrWhiteSpace(dto.TruckNumber))
            throw new BusinessRuleException("Truck number is required before dispatch.");
    }
}

