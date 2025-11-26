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
    public class CarrierQueryService : ICarrierQueryService
    {
        private readonly ICarrierQueryRepository _repo;

        public CarrierQueryService(ICarrierQueryRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<Carrier>> GetPagedAsync(CarrierQueryParameters parameters)
        {
            if (parameters.PageSize > 100)
                parameters.PageSize = 100;

            return await _repo.GetPagedAsync(parameters);
        }
    }
}
