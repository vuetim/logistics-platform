using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Domain.Common.Delays;
using LogisticsPlatform.Domain.Enums;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Services
{
    public class DelayResponsibilityService : IDelayResponsibilityService
    {
        private readonly IDelayResponsibilityRepository _repo;
        private readonly ILoadStopRepository _stopRepo;

        public DelayResponsibilityService(
            IDelayResponsibilityRepository repo,
            ILoadStopRepository stopRepo)
        {
            _repo = repo;
            _stopRepo = stopRepo;
        }

        public async Task AssignAsync(
            Guid loadStopId,
            DelayResponsibilityType responsibility,
            string? reason,
            Guid userId)
        {
            var stop = await _stopRepo.GetByIdWithLoadAsync(loadStopId)
                ?? throw new NotFoundException("Load stop not found.");

            var entity = new DelayResponsibility
            {
                LoadId = stop.LoadId,
                LoadStopId = stop.Id,
                Responsibility = responsibility,
                Reason = reason,
                AssignedByUserId = userId,
                IsFinal = true
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task<List<DelayResponsibilityDto>> GetByLoadAsync(Guid loadId)
        {
            var items = await _repo.GetByLoadAsync(loadId);

            return items.Select(x =>
            {
                var profile = DelayResponsibilityProfiles.From(x.Responsibility);

                return new DelayResponsibilityDto
                {
                    Id = x.Id,
                    LoadId = x.LoadId,
                    LoadStopId = x.LoadStopId,

                    FaultType = profile.FaultType,
                    ResponsibleParty = profile.ResponsibleParty,

                    MinutesLate = null, // lidhet me LoadStop në hapin tjetër
                    Reason = x.Reason ?? string.Empty,

                    IsManualOverride = true,
                    CreatedAt = x.AssignedAt
                };
            }).ToList();
        }
    }
}
