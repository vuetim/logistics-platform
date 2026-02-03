using LogisticsPlatform.Application.DTOs.Common;
using LogisticsPlatform.Application.DTOs.Customers.Contacts;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;

public class CustomerContactService : ICustomerContactService
{
    private readonly ICustomerContactRepository _contacts;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _uow;

    public CustomerContactService(
        ICustomerContactRepository contacts,
        ICustomerRepository customers,
        IUnitOfWork uow)
    {
        _contacts = contacts;
        _customers = customers;
        _uow = uow;
    }

    // =========================
    // CREATE
    // =========================
    public async Task<CustomerContactDto> CreateAsync(CreateCustomerContactDto dto)
    {
        var customer = await _customers.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            throw new Exception("Customer not found");

        if (dto.IsPrimary)
        {
            var primaries = await _contacts.GetPrimaryByCustomerAsync(dto.CustomerId);

            foreach (var p in primaries)
            {
                p.IsPrimary = false;
                _contacts.Update(p);
            }
        }

        var contact = new CustomerContact
        {
            CustomerId = dto.CustomerId,
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Position = dto.Position,
            IsPrimary = dto.IsPrimary,
            IsActive = true
        };

        await _contacts.AddAsync(contact);

        await _uow.SaveChangesAsync();

        return Map(contact);
    }

    // =========================
    // UPDATE
    // =========================
    public async Task<CustomerContactDto?> UpdateAsync(Guid id, UpdateCustomerContactDto dto)
    {
        var contact = await _contacts.GetByIdAsync(id);
        if (contact == null) return null;

        if (dto.IsPrimary == true)
        {
            var primaries = await _contacts.GetPrimaryByCustomerAsync(contact.CustomerId);

            foreach (var p in primaries)
            {
                p.IsPrimary = false;
                _contacts.Update(p);
            }
        }

        contact.FullName = dto.FullName ?? contact.FullName;
        contact.Email = dto.Email ?? contact.Email;
        contact.Phone = dto.Phone ?? contact.Phone;
        contact.Position = dto.Position ?? contact.Position;

        _contacts.Update(contact);

        await _uow.SaveChangesAsync();

        return Map(contact);
    }

    // =========================
    // DELETE (soft)
    // =========================
    public async Task<bool> DeleteAsync(Guid id)
    {
        var contact = await _contacts.GetByIdAsync(id);
        if (contact == null) return false;

        contact.IsActive = false;
        contact.IsPrimary = false;

        _contacts.Update(contact);

        await _uow.SaveChangesAsync();

        return true;
    }

    // =========================
    // GET BY CUSTOMER
    // =========================
    public async Task<IReadOnlyList<CustomerContactDto>> GetByCustomerAsync(Guid customerId)
    {
        var contacts = await _contacts.GetByCustomerAsync(customerId);

        return contacts
            .Where(x => x.IsActive)
            .Select(Map)
            .ToList();
    }

    // =========================
    private static CustomerContactDto Map(CustomerContact c) => new()
    {
        Id = c.Id,
        CustomerId = c.CustomerId,
        FullName = c.FullName,
        Email = c.Email,
        Phone = c.Phone,
        Position = c.Position,
        IsPrimary = c.IsPrimary,
        IsActive = c.IsActive
    };
}
