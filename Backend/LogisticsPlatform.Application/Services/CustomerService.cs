using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customers;

        public CustomerService(ICustomerRepository customers)
        {
            _customers = customers;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customers.GetAllAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _customers.GetByIdAsync(id);
        }

        public async Task<Customer> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                PaymentTermsDays = dto.PaymentTermsDays
            };

            await _customers.AddAsync(customer);
            await _customers.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer?> UpdateAsync(Guid id, UpdateCustomerDto dto)
        {
            var customer = await _customers.GetByIdAsync(id);
            if (customer == null) return null;

            customer.Name = dto.Name ?? customer.Name;
            customer.Email = dto.Email ?? customer.Email;
            customer.Phone = dto.Phone ?? customer.Phone;
            customer.Address = dto.Address ?? customer.Address;
            customer.PaymentTermsDays = dto.PaymentTermsDays;
            await _customers.UpdateAsync(customer);
            await _customers.SaveChangesAsync();

            return customer;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var customer = await _customers.GetByIdAsync(id);
            if (customer == null) return false;

            await _customers.DeleteAsync(customer);
            await _customers.SaveChangesAsync();

            return true;
        }
    }
}
