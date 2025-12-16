using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories
{
    public class OrderEquipmentRequirementRepository : IOrderEquipmentRequirementRepository
    {
        private readonly AppDbContext _context;

        public OrderEquipmentRequirementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderEquipmentRequirement requirement)
        {
            await _context.OrderEquipmentRequirements.AddAsync(requirement);
        }

        public Task UpdateAsync(OrderEquipmentRequirement requirement)
        {
            _context.OrderEquipmentRequirements.Update(requirement);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(OrderEquipmentRequirement requirement)
        {
            _context.OrderEquipmentRequirements.Remove(requirement);
            return Task.CompletedTask;
        }

        public async Task<OrderEquipmentRequirement?> GetByIdAsync(Guid id)
        {
            return await _context.OrderEquipmentRequirements
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<OrderEquipmentRequirement>> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.OrderEquipmentRequirements
                .Where(x => x.OrderId == orderId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
