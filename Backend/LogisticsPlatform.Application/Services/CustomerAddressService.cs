using LogisticsPlatform.Application.DTOs.Common;
using LogisticsPlatform.Application.DTOs.Customers.Addresses;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;

public class CustomerAddressService : ICustomerAddressService
{
    private readonly ICustomerAddressRepository _repo;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _uow;

    public CustomerAddressService(
        ICustomerAddressRepository repo,
        ICustomerRepository customers,
        IUnitOfWork uow)
    {
        _repo = repo;
        _customers = customers;
        _uow = uow;
    }

    // =========================
    // CREATE
    // =========================
    public async Task<CustomerAddressDto> CreateAsync(CreateCustomerAddressDto dto)
    {
        var customer = await _customers.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            throw new Exception("Customer not found");

        if (dto.IsPrimary)
        {
            var primaries = await _repo.GetPrimaryByCustomerAsync(dto.CustomerId);

            foreach (var p in primaries)
            {
                p.IsPrimary = false;
                _repo.Update(p);
            }
        }

        var address = new CustomerAddress
        {
            CustomerId = dto.CustomerId,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            PostalCode = dto.PostalCode,
            Type = dto.Type,
            IsPrimary = dto.IsPrimary,
            IsActive = true
        };

        await _repo.AddAsync(address);

        await _uow.SaveChangesAsync();

        return Map(address);
    }

    // =========================
    // GET BY CUSTOMER
    // =========================
    public async Task<IReadOnlyList<CustomerAddressDto>> GetByCustomerAsync(Guid customerId)
    {
        var addresses = await _repo.GetByCustomerAsync(customerId);

        return addresses
            .Where(a => a.IsActive)
            .Select(Map)
            .ToList();
    }

    // =========================
    // UPDATE
    // =========================
    public async Task<CustomerAddressDto?> UpdateAsync(Guid id, UpdateCustomerAddressDto dto)
    {
        var address = await _repo.GetByIdAsync(id);
        if (address == null) return null;

        if (dto.IsPrimary == true)
        {
            var primaries = await _repo.GetPrimaryByCustomerAsync(address.CustomerId);

            foreach (var p in primaries)
            {
                p.IsPrimary = false;
                _repo.Update(p);
            }
        }

        address.AddressLine1 = dto.AddressLine1 ?? address.AddressLine1;
        address.AddressLine2 = dto.AddressLine2 ?? address.AddressLine2;
        address.City = dto.City ?? address.City;
        address.State = dto.State ?? address.State;
        address.Country = dto.Country ?? address.Country;
        address.PostalCode = dto.PostalCode ?? address.PostalCode;
        address.Type = dto.Type ?? address.Type;

        if (dto.IsPrimary.HasValue)
            address.IsPrimary = dto.IsPrimary.Value;

        if (dto.IsActive.HasValue)
            address.IsActive = dto.IsActive.Value;

        _repo.Update(address);

        await _uow.SaveChangesAsync();

        return Map(address);
    }

    // =========================
    // DELETE (soft)
    // =========================
    public async Task<bool> DeleteAsync(Guid id)
    {
        var address = await _repo.GetByIdAsync(id);
        if (address == null) return false;

        address.IsActive = false;
        address.IsPrimary = false;

        _repo.Update(address);

        await _uow.SaveChangesAsync();

        return true;
    }

    // =========================
    private static CustomerAddressDto Map(CustomerAddress a) => new()
    {
        Id = a.Id,
        AddressLine1 = a.AddressLine1,
        AddressLine2 = a.AddressLine2,
        City = a.City,
        State = a.State,
        Country = a.Country,
        PostalCode = a.PostalCode,
        Type = a.Type,
        IsPrimary = a.IsPrimary,
        IsActive = a.IsActive
    };
}
