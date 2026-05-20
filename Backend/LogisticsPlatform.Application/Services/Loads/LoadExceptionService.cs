using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs.Loads.Exceptions;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Notifications;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;
using NotFoundException = SendGrid.Helpers.Errors.Model.NotFoundException;

namespace LogisticsPlatform.Application.Services.Loads;

public class LoadExceptionService : ILoadExceptionService
{
    private readonly ILoadRepository _loadRepository;
    private readonly ILoadExceptionRepository _exceptionRepository;
    private readonly IPermissionService _permissionService;
    private readonly INotificationService _notifications;

    public LoadExceptionService(
        ILoadRepository loadRepository,
        ILoadExceptionRepository exceptionRepository,
        IPermissionService permissionService,
        INotificationService notifications)
    {
        _loadRepository = loadRepository;
        _exceptionRepository = exceptionRepository;
        _permissionService = permissionService;
        _notifications = notifications;
    }

    public async Task<IReadOnlyList<LoadExceptionDto>> GetByLoadAsync(Guid loadId, Guid userId)
    {
        await EnsurePermissionAsync(userId, Permission.LoadException_View, "You are not allowed to view load exceptions.");

        var items = await _exceptionRepository.GetByLoadAsync(loadId);
        return items.Select(Map).ToList();
    }

    public async Task<Guid> CreateAsync(Guid loadId, CreateLoadExceptionRequest request, Guid userId)
    {
        await EnsurePermissionAsync(userId, Permission.LoadException_Create, "You are not allowed to create load exceptions.");

        _ = await _loadRepository.GetByIdAsync(loadId)
            ?? throw new NotFoundException("Load not found.");

        var item = new LoadException
        {
            LoadId = loadId,
            LoadStopId = request.LoadStopId,
            OrderId = request.OrderId,
            ExceptionKey = request.ExceptionKey.Trim(),
            ExceptionValue = request.ExceptionValue.Trim(),
            ReasonKey = request.ReasonKey,
            ReasonValue = request.ReasonValue,
            EdiReasonCode = request.EdiReasonCode,
            ResponsiblePartyKey = request.ResponsiblePartyKey,
            ResponsiblePartyValue = request.ResponsiblePartyValue,
            Status = request.Status,
            Description = request.Description,
            AffectedItemName = request.AffectedItemName,
            AffectedItemReference = request.AffectedItemReference,
            Quantity = request.Quantity,
            Unit = request.Unit,
            OccurredAt = request.OccurredAt ?? DateTime.UtcNow,
            CreatedByUserId = userId
        };

        await _exceptionRepository.AddAsync(item);
        await _notifications.NotifyLoadExceptionEventAsync(
            loadId,
            userId,
            $"Exception created: {item.ExceptionValue}");

        return item.Id;
    }

    public async Task UpdateAsync(Guid loadId, Guid exceptionId, UpdateLoadExceptionRequest request, Guid userId)
    {
        await EnsurePermissionAsync(userId, Permission.LoadException_Update, "You are not allowed to update load exceptions.");

        var item = await _exceptionRepository.GetByIdForLoadAsync(loadId, exceptionId)
            ?? throw new NotFoundException("Exception not found.");

        item.ExceptionKey = request.ExceptionKey?.Trim() ?? item.ExceptionKey;
        item.ExceptionValue = request.ExceptionValue?.Trim() ?? item.ExceptionValue;
        item.ReasonKey = request.ReasonKey ?? item.ReasonKey;
        item.ReasonValue = request.ReasonValue ?? item.ReasonValue;
        item.EdiReasonCode = request.EdiReasonCode ?? item.EdiReasonCode;
        item.ResponsiblePartyKey = request.ResponsiblePartyKey ?? item.ResponsiblePartyKey;
        item.ResponsiblePartyValue = request.ResponsiblePartyValue ?? item.ResponsiblePartyValue;
        item.Description = request.Description ?? item.Description;
        item.ResolutionNotes = request.ResolutionNotes ?? item.ResolutionNotes;
        item.Status = request.Status ?? item.Status;

        if (item.Status == LoadExceptionStatus.Resolved && item.ResolvedAt == null)
            item.ResolvedAt = DateTime.UtcNow;

        await _exceptionRepository.UpdateAsync(item);
        await _notifications.NotifyLoadExceptionEventAsync(
            loadId,
            userId,
            $"Exception updated: {item.ExceptionValue} ({item.Status})");
    }

    private async Task EnsurePermissionAsync(Guid userId, Permission permission, string message)
    {
        if (!await _permissionService.HasPermissionAsync(userId, permission))
            throw new ForbiddenException(message);
    }

    private static LoadExceptionDto Map(LoadException item)
    {
        return new LoadExceptionDto
        {
            Id = item.Id,
            LoadId = item.LoadId,
            LoadStopId = item.LoadStopId,
            OrderId = item.OrderId,
            ExceptionKey = item.ExceptionKey,
            ExceptionValue = item.ExceptionValue,
            ReasonKey = item.ReasonKey,
            ReasonValue = item.ReasonValue,
            EdiReasonCode = item.EdiReasonCode,
            ResponsiblePartyKey = item.ResponsiblePartyKey,
            ResponsiblePartyValue = item.ResponsiblePartyValue,
            Status = item.Status,
            Description = item.Description,
            ResolutionNotes = item.ResolutionNotes,
            AffectedItemName = item.AffectedItemName,
            AffectedItemReference = item.AffectedItemReference,
            Quantity = item.Quantity,
            Unit = item.Unit,
            OccurredAt = item.OccurredAt,
            ResolvedAt = item.ResolvedAt
        };
    }
}
