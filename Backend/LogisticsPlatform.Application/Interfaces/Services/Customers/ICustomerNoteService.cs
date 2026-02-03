using LogisticsPlatform.Application.DTOs.Customers.Notes;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Interfaces.Services.Customers
{
    public interface ICustomerNoteService
    {
        Task<CustomerNoteDto> CreateAsync(
            CreateCustomerNoteDto dto,
            Guid userId
        );

        Task<CustomerNoteDto?> UpdateAsync(
            Guid id,
            UpdateCustomerNoteDto dto
        );

        Task<bool> DeleteAsync(Guid id);

        Task<IReadOnlyList<CustomerNoteDto>> GetByCustomerAsync(Guid customerId);
    }

}
