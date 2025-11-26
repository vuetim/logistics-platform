using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ICarrierDocumentQueryService
    {
        Task<PagedResult<CarrierDocument>> GetPagedAsync(CarrierDocumentQueryParameters parameters);
    }
}
