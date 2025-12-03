using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ILoadStatusCalculatorService
    {
        LoadStatus Calculate(Load load);
    }
}
