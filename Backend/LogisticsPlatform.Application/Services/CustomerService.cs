using LogisticsPlatform.Application.DTOs.Common;
using LogisticsPlatform.Application.DTOs.Customers;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Domain.Entities;
using SendGrid.Helpers.Mail;

namespace LogisticsPlatform.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customers;
        private readonly ICustomerAddressService _addressService;
        private readonly ICustomerContactService _contactService;
        private readonly ICustomerNoteService _noteService;
        private readonly IUnitOfWork _uow;

        public CustomerService(
            ICustomerRepository customers,
            ICustomerAddressService addressService,
            ICustomerContactService contactService,
            ICustomerNoteService noteService,
            IUnitOfWork uow
        )
        {
            _customers = customers;
            _addressService = addressService;
            _contactService = contactService;
            _noteService = noteService;
            _uow = uow;
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
        public async Task<Customer> CreateFullAsync(CreateCustomerFullDto dto, Guid userId)
        {
            await _uow.BeginAsync();

            try
            {
                var customer = new Customer
                {
                    Name = dto.Customer.Name,
                    Email = dto.Customer.Email,
                    Phone = dto.Customer.Phone,
                    Address = dto.Customer.Address,
                    PaymentTermsDays = dto.Customer.PaymentTermsDays,
                    IsActive = dto.Customer.IsActive
                };

                await _customers.AddAsync(customer);
                await _customers.SaveChangesAsync();

                foreach (var address in dto.Addresses)
                {
                    address.CustomerId = customer.Id;
                    await _addressService.CreateAsync(address);
                }

                foreach (var c in dto.Contacts)
                {
                    c.CustomerId = customer.Id;
                    await _contactService.CreateAsync(c);
                }

                foreach (var n in dto.Notes)
                {
                    n.CustomerId = customer.Id;
                    await _noteService.CreateAsync(n, userId);
                }


                await _uow.CommitAsync();
                return customer;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
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
