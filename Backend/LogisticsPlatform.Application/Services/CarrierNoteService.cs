using LogisticsPlatform.Application.DTOs.Carriers.Notes;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services
{
    public class CarrierNoteService : ICarrierNoteService
    {
        private readonly ICarrierNoteRepository _notes;
        private readonly ICarrierRepository _carriers;

        public CarrierNoteService(
            ICarrierNoteRepository notes,
            ICarrierRepository carriers)
        {
            _notes = notes;
            _carriers = carriers;
        }

        public async Task<IEnumerable<CarrierNote>> GetByCarrierAsync(Guid carrierId)
        {
            return await _notes.GetByCarrierIdAsync(carrierId);
        }

        public async Task<CarrierNote> CreateAsync(Guid userId, CreateCarrierNoteDto dto)
        {
            var carrier = await _carriers.GetByIdAsync(dto.CarrierId);
            if (carrier == null)
                throw new Exception("Carrier not found");

            var note = new CarrierNote
            {
                CarrierId = dto.CarrierId,
                Title = dto.Title,
                Message = dto.Message,
                CreatedByUserId = userId
            };

            await _notes.AddAsync(note);
            await _notes.SaveChangesAsync();

            return note;
        }

        public async Task<CarrierNote?> UpdateAsync(Guid id, UpdateCarrierNoteDto dto)
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
    }
}
