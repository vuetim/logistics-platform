


namespace LogisticsPlatform.Application.Services
{


    public class CustomerAddressService : ICustomerAddressService


    {
        private readonly ICustomerAddressRepository _repo;

        public CustomerAddressService(ICustomerAddressRepository repo)
        {
            _repo = repo;
        }

        public async Task<CustomerAddress> CreateAsync(CreateCustomerAddressDto dto)
        {
            if (dto.IsPrimary)
            {
                var primaries = await _repo.GetPrimaryByCustomerAsync(dto.CustomerId);

                foreach (var a in primaries)
                {
                    a.IsPrimary = false;
                    await _repo.UpdateAsync(a);
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

            if (dto.IsPrimary == true)
            {
                var primaries = await _repo.GetPrimaryByCustomerAsync(address.CustomerId);

                foreach (var p in primaries)
                {
                    p.IsPrimary = false;
                    await _repo.UpdateAsync(p);
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

            await _repo.UpdateAsync(address);
            await _repo.SaveChangesAsync();

            address.Customer = null;
            return address;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var address = await _repo.GetByIdAsync(id);
            if (address == null) return false;
            address.IsActive = false;
            address.IsPrimary = false;
            await _repo.DeleteAsync(address);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}