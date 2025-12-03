using LogisticsPlatform.Application.DTOs.Loads.LoadStop;
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
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,

                PlannedArrivalFrom = dto.PlannedArrivalFrom,
                PlannedArrivalTo = dto.PlannedArrivalTo,

                PlannedDepartureFrom = dto.PlannedDepartureFrom,
                PlannedDepartureTo = dto.PlannedDepartureTo,

                AppointmentType = dto.AppointmentType,
                FlexMinutes = dto.FlexMinutes,

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
            stop.AddressLine1 = dto.AddressLine1;
            stop.AddressLine2 = dto.AddressLine2;
            stop.City = dto.City;
            stop.State = dto.State;
            stop.PostalCode = dto.PostalCode;
            stop.Country = dto.Country;

            stop.PlannedArrivalFrom = dto.PlannedArrivalFrom;
            stop.PlannedArrivalTo = dto.PlannedArrivalTo;

            stop.PlannedDepartureFrom = dto.PlannedDepartureFrom;
            stop.PlannedDepartureTo = dto.PlannedDepartureTo;

            stop.AppointmentType = dto.AppointmentType;
            stop.FlexMinutes = dto.FlexMinutes;

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
