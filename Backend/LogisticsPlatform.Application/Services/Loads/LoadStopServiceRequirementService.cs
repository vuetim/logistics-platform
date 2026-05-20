using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads.LoadStopServices;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;
using NotFoundException = SendGrid.Helpers.Errors.Model.NotFoundException;

namespace LogisticsPlatform.Application.Services.Loads;

public class LoadStopServiceRequirementService : ILoadStopServiceRequirementService
{
    private readonly ILoadStopRepository _stopRepository;
    private readonly ILoadStopServiceRequirementRepository _serviceRepository;
    private readonly IPermissionService _permissionService;

    public LoadStopServiceRequirementService(
        ILoadStopRepository stopRepository,
        ILoadStopServiceRequirementRepository serviceRepository,
        IPermissionService permissionService)
    {
        _stopRepository = stopRepository;
        _serviceRepository = serviceRepository;
        _permissionService = permissionService;
    }

    public async Task<IReadOnlyList<LoadStopServiceDto>> GetByStopAsync(Guid stopId, Guid userId)
    {
        await EnsurePermissionAsync(userId, Permission.LoadStopService_View, "You are not allowed to view stop services.");

        var items = await _serviceRepository.GetByStopAsync(stopId);
        return items.Select(Map).ToList();
    }

    public async Task<Guid> CreateAsync(Guid stopId, CreateLoadStopServiceRequest request, Guid userId)
    {
        await EnsurePermissionAsync(userId, Permission.LoadStopService_Create, "You are not allowed to create stop services.");

        var stop = await _stopRepository.GetByIdWithLoadAsync(stopId)
            ?? throw new NotFoundException("Load stop not found.");
        await EnsureCompletedCorrectionAllowedAsync(stop.Load, userId);

        var item = new LoadStopServiceRequirement
        {
            LoadStopId = stopId,
            ServiceKey = request.ServiceKey.Trim(),
            ServiceValue = request.ServiceValue.Trim(),
            Notes = request.Notes,
            IsPickupService = request.IsPickupService,
            IsDeliveryService = request.IsDeliveryService
        };

        await _serviceRepository.AddAsync(item);
        await _serviceRepository.SaveChangesAsync();

        return item.Id;
    }

    public async Task DeleteAsync(Guid stopId, Guid serviceId, Guid userId)
    {
        await EnsurePermissionAsync(userId, Permission.LoadStopService_Delete, "You are not allowed to delete stop services.");

        var item = await _serviceRepository.GetByIdForStopWithLoadAsync(stopId, serviceId)
            ?? throw new NotFoundException("Stop service not found.");
        await EnsureCompletedCorrectionAllowedAsync(item.LoadStop.Load, userId);

        await _serviceRepository.DeleteAsync(item);
        await _serviceRepository.SaveChangesAsync();
    }

    private async Task EnsureCompletedCorrectionAllowedAsync(Load load, Guid userId)
    {
        if (load.Status != LoadStatus.Completed)
            return;

        if (!await _permissionService.HasPermissionAsync(userId, Permission.Load_CompletedCorrection))
            throw new BusinessRuleException("Completed load stop services are locked. Correction permission is required.");
    }

    private async Task EnsurePermissionAsync(Guid userId, Permission permission, string message)
    {
        if (!await _permissionService.HasPermissionAsync(userId, permission))
            throw new ForbiddenException(message);
    }

    private static LoadStopServiceDto Map(LoadStopServiceRequirement item)
    {
        return new LoadStopServiceDto
        {
            Id = item.Id,
            LoadStopId = item.LoadStopId,
            ServiceKey = item.ServiceKey,
            ServiceValue = item.ServiceValue,
            Notes = item.Notes,
            IsPickupService = item.IsPickupService,
            IsDeliveryService = item.IsDeliveryService
        };
    }
}
