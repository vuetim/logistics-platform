using LogisticsPlatform.Application.DTOs.Common;
using LogisticsPlatform.Application.DTOs.Customers.Notes;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Entities;

public class CustomerNoteService : ICustomerNoteService
{
    private readonly ICustomerNoteRepository _notes;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _uow;

    public CustomerNoteService(
        ICustomerNoteRepository notes,
        ICustomerRepository customers,
        IUnitOfWork uow)
    {
        _notes = notes;
        _customers = customers;
        _uow = uow;
    }

    // =========================
    // CREATE
    // =========================
    public async Task<CustomerNoteDto> CreateAsync(CreateCustomerNoteDto dto, Guid userId)
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

        await _uow.SaveChangesAsync();

        return Map(note);
    }

    // =========================
    // UPDATE
    // =========================
    public async Task<CustomerNoteDto?> UpdateAsync(Guid id, UpdateCustomerNoteDto dto)
    {
        var note = await _notes.GetByIdAsync(id);
        if (note == null) return null;

        note.Title = dto.Title ?? note.Title;
        note.Message = dto.Message ?? note.Message;

        _notes.Update(note);

        await _uow.SaveChangesAsync();

        return Map(note);
    }

    // =========================
    // DELETE
    // =========================
    public async Task<bool> DeleteAsync(Guid id)
    {
        var note = await _notes.GetByIdAsync(id);
        if (note == null) return false;

        _notes.Remove(note);

        await _uow.SaveChangesAsync();

        return true;
    }

    // =========================
    // GET BY CUSTOMER
    // =========================
    public async Task<IReadOnlyList<CustomerNoteDto>> GetByCustomerAsync(Guid customerId)
    {
        var notes = await _notes.GetByCustomerAsync(customerId);

        return notes.Select(Map).ToList();
    }

    // =========================
    private static CustomerNoteDto Map(CustomerNote n) => new()
    {
        Id = n.Id,
        CustomerId = n.CustomerId,
        Title = n.Title,
        Message = n.Message,
        CreatedByUserId = n.CreatedByUserId,
        CreatedAt = n.CreatedAt,
        CreatedByName = n.CreatedByUser?.FullName
    };
}
