using LogisticsPlatform.Application.Common.Exceptions;
using LogisticsPlatform.Application.DTOs;
using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;
using LogisticsPlatform.Domain.Security;
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

        public CarrierAssignmentService(
            ILoadRepository loadRepo,
            ICarrierRepository carrierRepo,
            ILoadCarrierAssignmentRepository assignmentRepo
           )
        {
            _loadRepo = loadRepo;
            _carrierRepo = carrierRepo;
            _assignmentRepo = assignmentRepo;
           
        }

        // ============================
        // 1️⃣ TENDER
        // ============================
        public async Task<Guid> TenderAsync(TenderCarrierDto dto, Guid userId)
        {
            

            var load = await _loadRepo.GetByIdAsync(dto.LoadId)
                ?? throw new NotFoundException("Load not found");

            if (load.Status != LoadStatus.Draft)
                throw new BusinessRuleException("Only draft loads can be tendered");

            var carrier = await _carrierRepo.GetByIdAsync(dto.CarrierId)
                ?? throw new NotFoundException("Carrier not found");
            if (load.Status != LoadStatus.Draft && load.Status != LoadStatus.Rejected)
                throw new BusinessRuleException("Load cannot be tendered in this state");

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
                Status = AssignmentStatus.Tendered,
                IsActive = true,
                CreatedByUserId = userId
            };


            await _assignmentRepo.AddAsync(assignment);

            // 🔥 change load status
            load.Status = LoadStatus.Tendered;
            await _loadRepo.UpdateAsync(load);

            await _assignmentRepo.SaveChangesAsync();

            return assignment.Id;
        }

        // ============================
        // 2️⃣ ACCEPT
        // ============================
        public async Task AcceptAsync(Guid assignmentId, Guid userId)
        {
            var assignment = await _assignmentRepo.GetByIdAsync(assignmentId)
                ?? throw new NotFoundException("Assignment not found");

            if (assignment.Status != AssignmentStatus.Tendered)
                throw new BusinessRuleException("Assignment cannot be accepted");

            assignment.Status = AssignmentStatus.Accepted;
            assignment.AcceptedAt = DateTime.UtcNow;

            var load = assignment.Load;
            load.CarrierId = assignment.CarrierId;
            load.CarrierRate = assignment.OfferedRate;
            load.Status = LoadStatus.Accepted;
            load.RateConfirmationNumber = assignment.RateConfirmationNumber;


            await _assignmentRepo.UpdateAsync(assignment);
            await _loadRepo.UpdateAsync(load);

            await _assignmentRepo.SaveChangesAsync();
        }

        // ============================
        // 3️⃣ REJECT
        // ============================
        public async Task RejectAsync(Guid assignmentId, Guid userId)
        {
            var assignment = await _assignmentRepo.GetByIdAsync(assignmentId)
                ?? throw new NotFoundException("Assignment not found");

            assignment.Status = AssignmentStatus.Rejected;
            assignment.RejectedAt = DateTime.UtcNow;
            assignment.IsActive = false;

            var load = assignment.Load;
            load.Status = LoadStatus.Draft;

            await _assignmentRepo.UpdateAsync(assignment);
            await _loadRepo.UpdateAsync(load);
            await _assignmentRepo.SaveChangesAsync();
        }
    }

}
