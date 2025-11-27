using LogisticsPlatform.Application.DTOs.LoadStop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ILoadStopService
    {
        Task AddAsync(Guid loadId, CreateLoadStopDto dto);
        Task UpdateAsync(Guid stopId, UpdateLoadStopDto dto);
        Task DeleteAsync(Guid stopId);
    }
}
