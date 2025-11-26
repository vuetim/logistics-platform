using LogisticsPlatform.Application.DTOs.Customers.Contacts;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services
{
    public class CustomerContactService : ICustomerContactService
    {
        private readonly ICustomerContactRepository _contacts;
        private readonly ICustomerRepository _customers;

        public CustomerContactService(
            ICustomerContactRepository contacts,
            ICustomerRepository customers)
        {
            _contacts = contacts;
            _customers = customers;
        }

        public async Task<CustomerContact> CreateAsync(CreateCustomerContactDto dto)
        {
            // validate customer exists
            var customer = await _customers.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found");
            if (!CarrierContactRoles.All.Contains(dto.Position))
                throw new Exception("Invalid role");

            var contact = new CustomerContact
            {
                CustomerId = dto.CustomerId,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,

                Position = dto.Position
            };

            await _contacts.AddAsync(contact);
            await _contacts.SaveChangesAsync();

            return contact;
        }

        public async Task<CustomerContact?> UpdateAsync(Guid id, UpdateCustomerContactDto dto)
        {
            var contact = await _contacts.GetByIdAsync(id);
            if (contact == null) return null;

            contact.FullName = dto.FullName ?? contact.FullName;
            contact.Email = dto.Email ?? contact.Email;
            contact.Phone = dto.Phone ?? contact.Phone;
            if (dto.Position != null)
            {
                if (!CarrierContactRoles.All.Contains(dto.Position))
                    throw new Exception("Invalid role");
                contact.Position = dto.Position ?? contact.Position;
            }
            await _contacts.UpdateAsync(contact);
            await _contacts.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var contact = await _contacts.GetByIdAsync(id);
            if (contact == null) return false;

            await _contacts.DeleteAsync(contact);
            await _contacts.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerContact>> GetByCustomerAsync(Guid customerId)
        {
            return await _contacts.GetByCustomerAsync(customerId);
        }
    }
}
