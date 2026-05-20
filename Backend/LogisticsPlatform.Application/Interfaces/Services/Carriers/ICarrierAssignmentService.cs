using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Application.DTOs.Carriers;

namespace LogisticsPlatform.Application.Interfaces.Services.Carriers
{
    public interface ICarrierAssignmentService
    {
        Task<IReadOnlyList<LoadCarrierAssignmentDto>> GetByLoadAsync(Guid loadId, Guid userId);
        Task<IReadOnlyList<OpenCarrierOfferDto>> GetOpenOffersAsync(Guid userId);
        Task<Guid> TenderAsync(TenderCarrierDto dto, Guid userId);
        Task AcceptAsync(Guid assignmentId, Guid userId);
        Task RejectAsync(Guid assignmentId, Guid userId);
        Task<PublicCarrierTenderDto> GetPublicTenderAsync(string token);
        Task AcceptPublicTenderAsync(string token, RespondCarrierTenderDto dto);
        Task RejectPublicTenderAsync(string token, RespondCarrierTenderDto dto);
    }
}
