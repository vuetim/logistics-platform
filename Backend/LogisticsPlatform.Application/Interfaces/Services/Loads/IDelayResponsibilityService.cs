using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Domain.Enums;


namespace LogisticsPlatform.Application.Interfaces.Services.Loads
{
    public interface IDelayResponsibilityService
    {
        Task AssignAsync(
            Guid loadStopId,
            DelayResponsibilityType responsibility,
         
            string? reason,
            Guid userId);
        Task<List<DelayResponsibilityDto>> GetByLoadAsync(Guid loadId);


    }

}
