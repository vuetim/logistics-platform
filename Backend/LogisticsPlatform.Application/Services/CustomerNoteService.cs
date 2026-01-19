using LogisticsPlatform.Application.DTOs.Customers.Notes;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services
{
    public class CustomerNoteService : ICustomerNoteService
    {
        private readonly ICustomerNoteRepository _notes;
        private readonly ICustomerRepository _customers;

        public CustomerNoteService(ICustomerNoteRepository notes, ICustomerRepository customers)
        {
            _notes = notes;
            _customers = customers;
        }

        public async Task<CustomerNote> CreateAsync( CreateCustomerNoteDto dto, Guid userId)
        {
            var customer = await _customers.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found");

            var note = new CustomerNote
            {
                CustomerId = dto.CustomerId,
                CreatedByUserId = userId,
                Title = dto.Title,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow

            };

            await _notes.AddAsync(note);
            await _notes.SaveChangesAsync();

            return note;
        }

        public async Task<CustomerNote?> UpdateAsync(Guid id, UpdateCustomerNoteDto dto)
        {
            var note = await _notes.GetByIdAsync(id);
            if (note == null) return null;

            note.Title = dto.Title ?? note.Title;
            note.Message = dto.Message ?? note.Message;

            await _notes.UpdateAsync(note);
            await _notes.SaveChangesAsync();

            return note;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var note = await _notes.GetByIdAsync(id);
            if (note == null) return false;

            await _notes.DeleteAsync(note);
            await _notes.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerNote>> GetByCustomerAsync(Guid customerId)
        {
            return await _notes.GetByCustomerAsync(customerId);
        }
    }
}
