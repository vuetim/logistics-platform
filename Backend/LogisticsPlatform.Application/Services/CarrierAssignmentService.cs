using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Application.DTOs.Carriers;
using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Notifications;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Application.Options;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForbiddenException = LogisticsPlatform.Application.Common.Exceptions.ForbiddenException;

namespace LogisticsPlatform.Application.Services
{
    public class CarrierAssignmentService : ICarrierAssignmentService
    {
        private readonly ILoadRepository _loadRepo;
        private readonly ICarrierRepository _carrierRepo;
        private readonly ILoadCarrierAssignmentRepository _assignmentRepo;
        private readonly IPermissionService _permission;
        private readonly INotificationService _notifications;
        private readonly IEmailService _email;
        private readonly IRateConfirmationService _rateConfirmation;
        private readonly FrontendOptions _frontend;

        public CarrierAssignmentService(
            ILoadRepository loadRepo,
            ICarrierRepository carrierRepo,
            ILoadCarrierAssignmentRepository assignmentRepo,
            IPermissionService permission,
            INotificationService notifications,
            IEmailService email,
            IRateConfirmationService rateConfirmation,
            IOptions<FrontendOptions> frontend
           )
        {
            _loadRepo = loadRepo;
            _carrierRepo = carrierRepo;
            _assignmentRepo = assignmentRepo;
            _permission = permission;
            _notifications = notifications;
            _email = email;
            _rateConfirmation = rateConfirmation;
            _frontend = frontend.Value;
           
        }

        public async Task<IReadOnlyList<LoadCarrierAssignmentDto>> GetByLoadAsync(Guid loadId, Guid userId)
        {
            if (!await HasAnyPermissionAsync(userId, Permission.CarrierOffer_View, Permission.CarrierOffer_View_All, Permission.Load_Tender))
                throw new ForbiddenException("You are not allowed to view carrier assignments.");

            var load = await _loadRepo.GetByIdAsync(loadId)
                ?? throw new NotFoundException("Load not found");

            var assignments = await _assignmentRepo.GetByLoadIdAsync(load.Id);

            return assignments.Select(x => new LoadCarrierAssignmentDto
            {
                Id = x.Id,
                LoadId = x.LoadId,
                CarrierId = x.CarrierId,
                CarrierName = x.Carrier?.Name ?? string.Empty,
                OfferedRate = x.OfferedRate,
                Currency = x.Currency,
                RateConfirmationNumber = x.RateConfirmationNumber,
                TenderMethod = x.TenderMethod,
                TenderNotes = x.TenderNotes,
                TenderExpiresAt = x.TenderExpiresAt,
                Status = x.Status,
                TenderedAt = x.TenderedAt,
                AcceptedAt = x.AcceptedAt,
                AcceptedByName = x.AcceptedByName,
                AcceptedByEmail = x.AcceptedByEmail,
                RejectedAt = x.RejectedAt,
                RejectedReason = x.RejectedReason,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<IReadOnlyList<OpenCarrierOfferDto>> GetOpenOffersAsync(Guid userId)
        {
            if (!await _permission.HasPermissionAsync(userId, Permission.CarrierOffer_View_All))
                throw new ForbiddenException("You are not allowed to view open carrier offers.");

            var assignments = await _assignmentRepo.GetOpenTenderedAsync();

            return assignments.Select(x => new OpenCarrierOfferDto
            {
                AssignmentId = x.Id,
                LoadId = x.LoadId,
                LoadNumber = x.Load.LoadNumber,
                LoadStatus = x.Load.Status.ToString(),
                CustomerName = x.Load.Customer.Name,
                Origin = x.Load.Origin,
                Destination = x.Load.Destination,
                CarrierId = x.CarrierId,
                CarrierName = x.Carrier.Name,
                OfferedRate = x.OfferedRate,
                Currency = x.Currency ?? "USD",
                RateConfirmationNumber = x.RateConfirmationNumber,
                TenderMethod = x.TenderMethod,
                TenderNotes = x.TenderNotes,
                TenderedAt = x.TenderedAt,
                TenderExpiresAt = x.TenderExpiresAt
            }).ToList();
        }

        public async Task<Guid> TenderAsync(TenderCarrierDto dto, Guid userId)
        {
            if (!await HasAnyPermissionAsync(userId, Permission.CarrierOffer_Create, Permission.Load_Tender))
                throw new ForbiddenException("You are not allowed to tender loads.");

            var load = await _loadRepo.GetByIdAsync(dto.LoadId)
                ?? throw new NotFoundException("Load not found");

            if (load.Status != LoadStatus.Draft && load.Status != LoadStatus.Rejected)
                throw new BusinessRuleException("Only draft or rejected loads can be tendered.");

            var carrier = await _carrierRepo.GetByIdAsync(dto.CarrierId)
                ?? throw new NotFoundException("Carrier not found");

            // deactivate previous assignments
            var active = await _assignmentRepo.GetActiveByLoadAsync(load.Id);
            if (active != null)
            {
                active.IsActive = false;
                await _assignmentRepo.UpdateAsync(active);
            }

            var assignment = new LoadCarrierAssignment
            {
                LoadId = load.Id,
                CarrierId = carrier.Id,
                OfferedRate = dto.OfferedRate,
                Currency = dto.Currency,
                RateConfirmationNumber = dto.RateConfirmationNumber,
                TenderMethod = string.IsNullOrWhiteSpace(dto.TenderMethod) ? "Manual" : dto.TenderMethod.Trim(),
                TenderNotes = dto.TenderNotes,
                TenderExpiresAt = dto.TenderExpiresAt,
                TenderToken = ShouldEmailTender(dto.TenderMethod) ? CreateToken() : null,
                TenderEmailTo = ShouldEmailTender(dto.TenderMethod) ? await ResolveTenderEmailAsync(carrier.Id, dto.EmailTo) : null,
                Status = AssignmentStatus.Tendered,
                IsActive = true,
                CreatedByUserId = userId
            };


            await _assignmentRepo.AddAsync(assignment);
            load.Status = LoadStatus.Tendered;
            await _loadRepo.UpdateAsync(load);

            await _assignmentRepo.SaveChangesAsync();
            if (ShouldEmailTender(assignment.TenderMethod))
            {
                await SendTenderEmailAsync(assignment);
            }
            await _notifications.NotifyCarrierTenderEventAsync(
                load.Id,
                userId,
                $"Tender sent to {carrier.Name} for {dto.OfferedRate:N2} {dto.Currency ?? "USD"}");

            return assignment.Id;
        }

        public async Task AcceptAsync(Guid assignmentId, Guid userId)
        {
            if (!await HasAnyPermissionAsync(userId, Permission.CarrierOffer_Accept, Permission.Load_Tender))
                throw new ForbiddenException("You are not allowed to accept carrier tenders.");

            var assignment = await _assignmentRepo.GetByIdAsync(assignmentId)
                ?? throw new NotFoundException("Assignment not found");

            if (assignment.Status != AssignmentStatus.Tendered)
                throw new BusinessRuleException("Assignment cannot be accepted");

            assignment.Status = AssignmentStatus.Accepted;
            assignment.AcceptedAt = DateTime.UtcNow;
            assignment.AcceptedByName ??= "Dispatcher";

            var load = assignment.Load;
            load.CarrierId = assignment.CarrierId;
            load.CarrierRate = assignment.OfferedRate;
            load.Status = LoadStatus.Accepted;
            load.RateConfirmationNumber = assignment.RateConfirmationNumber;


            await _assignmentRepo.UpdateAsync(assignment);
            await _loadRepo.UpdateAsync(load);

            await _assignmentRepo.SaveChangesAsync();
            await SendRateConfirmationEmailAsync(assignment);
            await _notifications.NotifyCarrierTenderEventAsync(
                load.Id,
                userId,
                $"{assignment.Carrier?.Name ?? "Carrier"} accepted tender for {assignment.OfferedRate:N2} {assignment.Currency ?? "USD"}");
        }

        public async Task<PublicCarrierTenderDto> GetPublicTenderAsync(string token)
        {
            var assignment = await GetTenderByTokenOrThrowAsync(token);
            return MapPublicTender(assignment);
        }

        public async Task AcceptPublicTenderAsync(string token, RespondCarrierTenderDto dto)
        {
            var assignment = await GetTenderByTokenOrThrowAsync(token);
            EnsureTenderCanBeResponded(assignment);

            assignment.Status = AssignmentStatus.Accepted;
            assignment.AcceptedAt = DateTime.UtcNow;
            assignment.AcceptedByName = dto.ContactName?.Trim();
            assignment.AcceptedByEmail = dto.ContactEmail?.Trim();
            assignment.AcceptedByPhone = dto.ContactPhone?.Trim();

            var load = assignment.Load;
            load.CarrierId = assignment.CarrierId;
            load.CarrierRate = assignment.OfferedRate;
            load.Status = LoadStatus.Accepted;
            load.RateConfirmationNumber = assignment.RateConfirmationNumber;

            await _assignmentRepo.UpdateAsync(assignment);
            await _loadRepo.UpdateAsync(load);
            await _assignmentRepo.SaveChangesAsync();
            await SendRateConfirmationEmailAsync(assignment);
            await _notifications.NotifyCarrierTenderEventAsync(
                load.Id,
                assignment.CreatedByUserId,
                $"{assignment.Carrier.Name} accepted tender via carrier link");
        }

        public async Task RejectPublicTenderAsync(string token, RespondCarrierTenderDto dto)
        {
            var assignment = await GetTenderByTokenOrThrowAsync(token);
            EnsureTenderCanBeResponded(assignment);

            assignment.Status = AssignmentStatus.Rejected;
            assignment.RejectedAt = DateTime.UtcNow;
            assignment.RejectedReason = string.IsNullOrWhiteSpace(dto.Notes)
                ? "Rejected by carrier"
                : dto.Notes.Trim();
            assignment.IsActive = false;
            assignment.AcceptedByName = dto.ContactName?.Trim();
            assignment.AcceptedByEmail = dto.ContactEmail?.Trim();
            assignment.AcceptedByPhone = dto.ContactPhone?.Trim();

            var load = assignment.Load;
            load.Status = LoadStatus.Draft;

            await _assignmentRepo.UpdateAsync(assignment);
            await _loadRepo.UpdateAsync(load);
            await _assignmentRepo.SaveChangesAsync();
            await _notifications.NotifyCarrierTenderEventAsync(
                load.Id,
                assignment.CreatedByUserId,
                $"{assignment.Carrier.Name} rejected tender via carrier link");
        }

        public async Task RejectAsync(Guid assignmentId, Guid userId)
        {
            if (!await HasAnyPermissionAsync(userId, Permission.CarrierOffer_Reject, Permission.Load_Tender))
                throw new ForbiddenException("You are not allowed to reject carrier tenders.");

            var assignment = await _assignmentRepo.GetByIdAsync(assignmentId)
                ?? throw new NotFoundException("Assignment not found");

            if (assignment.Status != AssignmentStatus.Tendered)
                throw new BusinessRuleException("Assignment cannot be rejected");

            assignment.Status = AssignmentStatus.Rejected;
            assignment.RejectedAt = DateTime.UtcNow;
            assignment.RejectedReason ??= "Rejected by dispatcher";
            assignment.IsActive = false;

            var load = assignment.Load;
            load.Status = LoadStatus.Draft;

            await _assignmentRepo.UpdateAsync(assignment);
            await _loadRepo.UpdateAsync(load);
            await _assignmentRepo.SaveChangesAsync();
            await _notifications.NotifyCarrierTenderEventAsync(
                load.Id,
                userId,
                $"{assignment.Carrier?.Name ?? "Carrier"} rejected tender");
        }

        private async Task<bool> HasAnyPermissionAsync(Guid userId, params Permission[] permissions)
        {
            foreach (var permission in permissions)
            {
                if (await _permission.HasPermissionAsync(userId, permission))
                    return true;
            }

            return false;
        }

        private async Task<string?> ResolveTenderEmailAsync(Guid carrierId, string? explicitEmail)
        {
            if (!string.IsNullOrWhiteSpace(explicitEmail))
                return explicitEmail.Trim();

            var carrier = await _carrierRepo.GetByIdAsync(carrierId)
                ?? throw new NotFoundException("Carrier not found");

            return string.IsNullOrWhiteSpace(carrier.Email) ? null : carrier.Email.Trim();
        }

        private async Task SendTenderEmailAsync(LoadCarrierAssignment assignment)
        {
            if (string.IsNullOrWhiteSpace(assignment.TenderEmailTo))
                throw new BusinessRuleException("Carrier tender email is missing.");

            var url = $"{_frontend.BaseUrl.TrimEnd('/')}/carrier-tenders/{assignment.TenderToken}";
            var body = $@"
                <p>Hello,</p>
                <p>You have a carrier tender for load <strong>{assignment.Load.LoadNumber}</strong>.</p>
                <p><strong>Lane:</strong> {assignment.Load.Origin} to {assignment.Load.Destination}</p>
                <p><strong>Rate:</strong> {(assignment.OfferedRate ?? 0):N2} {assignment.Currency ?? "USD"}</p>
                <p><strong>Expires:</strong> {assignment.TenderExpiresAt:g}</p>
                <p><a href=""{url}"">Review, accept, or reject tender</a></p>
                <p>{assignment.TenderNotes}</p>";

            await _email.SendAsync(
                assignment.TenderEmailTo,
                $"Carrier tender - Load {assignment.Load.LoadNumber}",
                body);

            assignment.TenderEmailSentAt = DateTime.UtcNow;
            await _assignmentRepo.UpdateAsync(assignment);
            await _assignmentRepo.SaveChangesAsync();
        }

        private async Task SendRateConfirmationEmailAsync(LoadCarrierAssignment assignment)
        {
            var to = assignment.AcceptedByEmail ?? assignment.TenderEmailTo ?? assignment.Carrier.Email;
            if (string.IsNullOrWhiteSpace(to)) return;

            var pdf = _rateConfirmation.GeneratePdf(assignment);
            await _email.SendAsync(
                to,
                $"Rate confirmation - Load {assignment.Load.LoadNumber}",
                $"<p>Attached is the rate confirmation for load <strong>{assignment.Load.LoadNumber}</strong>.</p>",
                $"rate-confirmation-{assignment.Load.LoadNumber}.pdf",
                pdf);
        }

        private async Task<LoadCarrierAssignment> GetTenderByTokenOrThrowAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new NotFoundException("Tender not found.");

            return await _assignmentRepo.GetByTenderTokenAsync(token.Trim())
                ?? throw new NotFoundException("Tender not found.");
        }

        private static void EnsureTenderCanBeResponded(LoadCarrierAssignment assignment)
        {
            if (assignment.Status != AssignmentStatus.Tendered || !assignment.IsActive)
                throw new BusinessRuleException("Tender is no longer open.");

            if (assignment.TenderExpiresAt.HasValue && assignment.TenderExpiresAt.Value < DateTime.UtcNow)
                throw new BusinessRuleException("Tender has expired.");
        }

        private static PublicCarrierTenderDto MapPublicTender(LoadCarrierAssignment assignment)
            => new()
            {
                AssignmentId = assignment.Id,
                LoadNumber = assignment.Load.LoadNumber,
                CustomerName = assignment.Load.Customer.Name,
                Origin = assignment.Load.Origin,
                Destination = assignment.Load.Destination,
                CarrierName = assignment.Carrier.Name,
                OfferedRate = assignment.OfferedRate,
                Currency = assignment.Currency ?? "USD",
                TenderNotes = assignment.TenderNotes,
                TenderExpiresAt = assignment.TenderExpiresAt,
                Status = assignment.Status.ToString(),
                Stops = assignment.Load.Stops
                    .OrderBy(x => x.Sequence)
                    .Select(x => new PublicCarrierTenderStopDto
                    {
                        Sequence = x.Sequence,
                        StopType = x.StopType.ToString(),
                        LocationName = x.LocationName,
                        City = x.City,
                        State = x.State,
                        Country = x.Country,
                        PlannedArrivalFrom = x.PlannedArrivalFrom,
                        PlannedArrivalTo = x.PlannedArrivalTo
                    })
                    .ToList()
            };

        private static bool ShouldEmailTender(string? method)
            => string.Equals(method, "Email", StringComparison.OrdinalIgnoreCase)
               || string.Equals(method, "Portal", StringComparison.OrdinalIgnoreCase);

        private static string CreateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(48);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }
    }

}
