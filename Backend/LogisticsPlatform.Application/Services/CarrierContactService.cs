using LogisticsPlatform.Application.DTOs.Carriers.Contacts;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Application.Services
{
    public class CarrierContactService : ICarrierContactService
    {
        private readonly ICarrierContactRepository _contacts;
        private readonly ICarrierRepository _carriers;

        public CarrierContactService(
            ICarrierContactRepository contacts,
            ICarrierRepository carriers)
        {
            _contacts = contacts;
            _carriers = carriers;
        }

        public async Task<CarrierContact> CreateAsync(CreateCarrierContactDto dto)
        {
            // Validate carrier exists
            var carrier = await _carriers.GetByIdAsync(dto.CarrierId);
            if (carrier == null)
                throw new Exception("Carrier not found");

            if (!CarrierContactRoles.All.Contains(dto.Role))
                throw new Exception("Invalid role");

            var contact = new CarrierContact
            {
                CarrierId = dto.CarrierId,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role
            };

            await _contacts.AddAsync(contact);
            await _contacts.SaveChangesAsync();

            return contact;
        }

        public async Task<CarrierContact?> UpdateAsync(Guid id, UpdateCarrierContactDto dto)
        {
            var contact = await _contacts.GetByIdAsync(id);
            if (contact == null)
                return null;

            contact.FullName = dto.FullName ?? contact.FullName;
            contact.Email = dto.Email ?? contact.Email;
            contact.Phone = dto.Phone ?? contact.Phone;

            if (dto.Role != null)
            {
                if (!CarrierContactRoles.All.Contains(dto.Role))
                    throw new Exception("Invalid role");

                contact.Role = dto.Role;
            }

            contact.UpdatedAt = DateTime.UtcNow;

            await _contacts.UpdateAsync(contact);
            await _contacts.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var contact = await _contacts.GetByIdAsync(id);
            if (contact == null)
                return false;

            await _contacts.DeleteAsync(contact);
            await _contacts.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CarrierContact>> GetByCarrierAsync(Guid carrierId)
        {
            return await _contacts.GetByCarrierAsync(carrierId);
        }
    }
}
