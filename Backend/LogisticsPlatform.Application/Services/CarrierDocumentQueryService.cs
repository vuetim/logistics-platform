using LogisticsPlatform.Application.DTOs.Pagination;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Services
{
    public class CarrierDocumentQueryService : ICarrierDocumentQueryService
    {
        private readonly ICarrierDocumentQueryRepository _repo;

        public CarrierDocumentQueryService(ICarrierDocumentQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<CarrierDocument>> GetPagedAsync(CarrierDocumentQueryParameters parameters)
        {
            if (parameters.PageSize > 100)
                parameters.PageSize = 100;

            return await _repo.GetPagedAsync(parameters);
        }
    }
}
