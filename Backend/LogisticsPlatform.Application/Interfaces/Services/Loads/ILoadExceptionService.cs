using LogisticsPlatform.Application.DTOs.Loads.Exceptions;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads;

public interface ILoadExceptionService
{
    Task<IReadOnlyList<LoadExceptionDto>> GetByLoadAsync(Guid loadId, Guid userId);
    Task<Guid> CreateAsync(Guid loadId, CreateLoadExceptionRequest request, Guid userId);
    Task UpdateAsync(Guid loadId, Guid exceptionId, UpdateLoadExceptionRequest request, Guid userId);
}
