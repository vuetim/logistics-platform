using LogisticsPlatform.Application.DTOs.Loads;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public interface ILoadDispatchPolicy
{
    void EnsureCanDispatch(Load load, DispatchLoadDto dto);
}

