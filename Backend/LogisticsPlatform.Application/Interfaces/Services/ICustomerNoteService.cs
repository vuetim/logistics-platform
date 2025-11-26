using LogisticsPlatform.Application.DTOs.Customers.Notes;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ICustomerNoteService
    {
        Task<CustomerNote> CreateAsync(Guid userId, CreateCustomerNoteDto dto);
        Task<CustomerNote?> UpdateAsync(Guid id, UpdateCustomerNoteDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CustomerNote>> GetByCustomerAsync(Guid customerId);
    }
}
