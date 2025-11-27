public class CustomerAddressService : ICustomerAddressService
{
    private readonly ICustomerAddressRepository _repo;

    public CustomerAddressService(ICustomerAddressRepository repo)
    {
        _repo = repo;
    }

    public async Task<CustomerAddress> CreateAsync(CreateCustomerAddressDto dto)
    {
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
            IsPrimary = dto.IsPrimary
        };

        await _repo.AddAsync(address);
        await _repo.SaveChangesAsync();

        address.Customer = null;
        return address;
    }

    public async Task<IEnumerable<CustomerAddress>> GetByCustomerAsync(Guid customerId)
    {
        var addresses = await _repo.GetByCustomerAsync(customerId);

        foreach (var a in addresses)
            a.Customer = null;

        return addresses;
    }

    public async Task<CustomerAddress?> UpdateAsync(Guid id, UpdateCustomerAddressDto dto)
    {
        var address = await _repo.GetByIdAsync(id);
        if (address == null) return null;

        address.AddressLine1 = dto.AddressLine1 ?? address.AddressLine1;
        address.AddressLine2 = dto.AddressLine2 ?? address.AddressLine2;
        address.City = dto.City ?? address.City;
        address.State = dto.State ?? address.State;
        address.Country = dto.Country ?? address.Country;
        address.PostalCode = dto.PostalCode ?? address.PostalCode;
        address.Type = dto.Type ?? address.Type;

        if (dto.IsPrimary.HasValue)
            address.IsPrimary = dto.IsPrimary.Value;

        await _repo.UpdateAsync(address);
        await _repo.SaveChangesAsync();

        address.Customer = null;
        return address;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var address = await _repo.GetByIdAsync(id);
        if (address == null) return false;

        await _repo.DeleteAsync(address);
        await _repo.SaveChangesAsync();
        return true;
    }
}
