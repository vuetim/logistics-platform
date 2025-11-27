using LogisticsPlatform.Application.DTOs.LoadStop;
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
    public class LoadStopService : ILoadStopService
    {
        private readonly ILoadStopRepository _repository;

        public LoadStopService(ILoadStopRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Guid loadId, CreateLoadStopDto dto)
        {
            var stop = new LoadStop
            {
                LoadId = loadId,
                StopType = dto.StopType,
                Sequence = dto.Sequence,
                LocationName = dto.LocationName,
                City = dto.City,
                State = dto.State,
                Zip = dto.Zip,
                PlannedDate = dto.PlannedDate,
                AppointmentFrom = dto.AppointmentFrom,
                AppointmentTo = dto.AppointmentTo,
                HasTime = dto.HasTime,
                Notes = dto.Notes
            };

            await _repository.AddAsync(stop);
        }

        public async Task UpdateAsync(Guid stopId, UpdateLoadStopDto dto)
        {
            var stop = await _repository.GetByIdAsync(stopId)
                ?? throw new Exception("Load stop not found");

            stop.StopType = dto.StopType;
            stop.Sequence = dto.Sequence;
            stop.LocationName = dto.LocationName;
            stop.City = dto.City;
            stop.State = dto.State;
            stop.Zip = dto.Zip;
            stop.PlannedDate = dto.PlannedDate;
            stop.AppointmentFrom = dto.AppointmentFrom;
            stop.AppointmentTo = dto.AppointmentTo;
            stop.HasTime = dto.HasTime;
            stop.Notes = dto.Notes;

            await _repository.UpdateAsync(stop);
        }

        public async Task DeleteAsync(Guid stopId)
        {
            var stop = await _repository.GetByIdAsync(stopId)
                ?? throw new Exception("Load stop not found");

            await _repository.DeleteAsync(stop);
        }
    }
}
