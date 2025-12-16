using LogisticsPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services.Carriers
{
    public interface ICarrierAssignmentService
    {
        Task<Guid> TenderAsync(TenderCarrierDto dto, Guid userId);
        Task AcceptAsync(Guid assignmentId, Guid userId);
        Task RejectAsync(Guid assignmentId, Guid userId);
    }

}
