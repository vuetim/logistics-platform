using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads.LoadStop;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
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
        private readonly ILoadRepository _loadRepo;
        private readonly ILoadStatusCalculatorService _statusCalculator;
        private readonly IOrderLoadSyncService _orderLoadSyncService;
        private readonly IPermissionService _permission;


        public LoadStopService(
            ILoadStopRepository repository,
            ILoadRepository loadRepo,
            ILoadStatusCalculatorService statusCalculator,
            IOrderLoadSyncService orderLoadSyncService,
            IPermissionService permission)
        {
            _repository = repository;
            _loadRepo = loadRepo;
            _statusCalculator = statusCalculator;
            _orderLoadSyncService = orderLoadSyncService;
            _permission = permission;
        }

        public async Task AddAsync(Guid loadId, CreateLoadStopDto dto, Guid userId)
        {
            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Update))
                throw new ForbiddenException("You are not allowed to update load stops.");

            var load = await _loadRepo.GetByIdAsync(loadId)
                ?? throw new SendGrid.Helpers.Errors.Model.NotFoundException("Load not found");
            await EnsureCompletedCorrectionAllowedAsync(load, userId);

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
                TimeZone = NormalizeTimeZone(dto.TimeZone),
                AppointmentStatus = dto.AppointmentStatus,
                AppointmentConfirmed = dto.AppointmentConfirmed,
                AppointmentConfirmationNumber = dto.AppointmentConfirmationNumber,
                AppointmentNumber = dto.AppointmentNumber,
                StopReference = dto.StopReference,
                PONumbers = dto.PONumbers,

                Notes = dto.Notes


            };

            await _repository.AddAsync(stop);
            await _loadRepo.SaveChangesAsync(); 


        }

        public async Task UpdateAsync(Guid stopId, UpdateLoadStopDto dto, Guid userId)
        {
            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Update))
                throw new ForbiddenException("You are not allowed to update load stops.");

            var stop = await _repository.GetByIdAsync(stopId)
                ?? throw new Exception("Load stop not found");
            var load = await _loadRepo.GetByIdAsync(stop.LoadId)
                ?? throw new SendGrid.Helpers.Errors.Model.NotFoundException("Load not found");
            await EnsureCompletedCorrectionAllowedAsync(load, userId);

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
            stop.TimeZone = NormalizeTimeZone(dto.TimeZone ?? stop.TimeZone);
            stop.AppointmentStatus = dto.AppointmentStatus;
            stop.AppointmentConfirmed = dto.AppointmentConfirmed;
            stop.AppointmentConfirmationNumber = dto.AppointmentConfirmationNumber;
            stop.AppointmentNumber = dto.AppointmentNumber;
            stop.StopReference = dto.StopReference;
            stop.PONumbers = dto.PONumbers;

            stop.Notes = dto.Notes;

            await _repository.UpdateAsync(stop);
            await _loadRepo.SaveChangesAsync(); 

        }
        //update status

        public async Task UpdateStatusAsync(
    Guid stopId,
    StopStatus newStatus,
    Guid userId)
        {
            var stop = await _repository.GetByIdAsync(stopId)
                ?? throw new Exception("Load stop not found");

            var load = await _loadRepo.GetByIdAsync(stop.LoadId)
                ?? throw new Exception("Load not found");

            //  Execution fillon vetëm pas dispatch
            if (load.Status < LoadStatus.Dispatched)
                throw new BusinessRuleException(
                    "Load must be dispatched before updating stop status.");

            //  Pickup vs Delivery rule
            if (stop.StopType == StopType.Delivery &&
                newStatus == StopStatus.Loaded)
                throw new BusinessRuleException(
                    "Delivery stop cannot be marked as loaded.");

            // Status order rule
            if (newStatus == StopStatus.Completed &&
                stop.Status != StopStatus.Arrived)
                throw new BusinessRuleException(
                    "Stop must be arrived before completed.");

            //  No backward transitions
            if (newStatus < stop.Status)
                throw new BusinessRuleException(
                    "Stop status cannot be reverted.");
            if (stop.Status == StopStatus.Completed &&
            stop.StopType == StopType.Pickup &&
            load.Status < LoadStatus.Dispatched)
            {
                throw new BusinessRuleException("Cannot complete pickup before dispatch");
            }
            // Apply
            stop.Status = newStatus;

            //  AUTO calculate load status
            load.Status = _statusCalculator.Calculate(load);

            await _repository.UpdateAsync(stop);
            await _loadRepo.UpdateAsync(load);
            await _loadRepo.SaveChangesAsync();
            await _orderLoadSyncService.SyncFromLoadAsync(load);
        }


        public async Task DeleteAsync(Guid stopId, Guid userId)
        {
            if (!await _permission.HasPermissionAsync(userId, Permission.Load_Update))
                throw new ForbiddenException("You are not allowed to update load stops.");

            var stop = await _repository.GetByIdAsync(stopId)
                ?? throw new Exception("Load stop not found");
            var load = await _loadRepo.GetByIdAsync(stop.LoadId)
                ?? throw new SendGrid.Helpers.Errors.Model.NotFoundException("Load not found");
            await EnsureCompletedCorrectionAllowedAsync(load, userId);

            await _repository.DeleteAsync(stop);
            await _loadRepo.SaveChangesAsync(); 

        }

        private static string NormalizeTimeZone(string? timeZone)
            => string.IsNullOrWhiteSpace(timeZone) ? "UTC" : timeZone.Trim();

        private async Task EnsureCompletedCorrectionAllowedAsync(Load load, Guid userId)
        {
            if (load.Status != LoadStatus.Completed)
                return;

            if (!await _permission.HasPermissionAsync(userId, Permission.Load_CompletedCorrection))
                throw new BusinessRuleException("Completed load stops are locked. Correction permission is required.");
        }
    }
}
