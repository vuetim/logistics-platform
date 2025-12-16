using LogisticsPlatform.Application.DTOs.Loads.LoadStop;
using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads
{
    public interface ILoadStopService
    {
        Task AddAsync(Guid loadId, CreateLoadStopDto dto);
        Task UpdateAsync(Guid stopId, UpdateLoadStopDto dto);
        Task UpdateStatusAsync(Guid stopId, StopStatus newStatus, Guid userId);

        Task DeleteAsync(Guid stopId);
    }
}
