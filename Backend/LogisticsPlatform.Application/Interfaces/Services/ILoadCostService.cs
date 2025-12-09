using LogisticsPlatform.Application.DTOs.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ILoadCostService
    {
        Task<LoadCostDto> GetAsync(Guid loadId);
        Task UpdateAsync(Guid loadId, UpdateLoadCostDto dto, Guid userId);
    }
}
