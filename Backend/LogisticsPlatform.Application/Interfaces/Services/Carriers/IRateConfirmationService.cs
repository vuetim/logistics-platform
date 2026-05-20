using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Carriers;

public interface IRateConfirmationService
{
    byte[] GeneratePdf(LoadCarrierAssignment assignment);
}
