using LogisticsPlatform.Application.DTOs.Carriers.Documents;
using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.Services
{
    public class CarrierDocumentService : ICarrierDocumentService
    {
        private readonly ICarrierDocumentRepository _docs;
        private readonly ICarrierRepository _carriers;

        public CarrierDocumentService(
            ICarrierDocumentRepository docs,
            ICarrierRepository carriers)
        {
            _docs = docs;
            _carriers = carriers;
        }

        public async Task<IEnumerable<CarrierDocument>> GetByCarrierAsync(Guid carrierId)
        {
            return await _docs.GetByCarrierAsync(carrierId);
        }

        public async Task<CarrierDocument> CreateAsync(Guid userId, CreateCarrierDocumentDto dto)
        {
            var carrier = await _carriers.GetByIdAsync(dto.CarrierId);
            if (carrier == null)
                throw new Exception("Carrier not found");

            var doc = new CarrierDocument
            {
                CarrierId = dto.CarrierId,
                FileName = dto.FileName,
                DocumentType = dto.DocumentType,
                FileUrl = dto.FileUrl,
                ExpiresAt = dto.ExpiresAt,
                UploadedByUserId = userId
            };

            await _docs.AddAsync(doc);
            await _docs.SaveChangesAsync();

            return doc;
        }

        public async Task<CarrierDocument?> UpdateAsync(Guid id, UpdateCarrierDocumentDto dto)
        {
            var doc = await _docs.GetByIdAsync(id);
            if (doc == null) return null;

            doc.FileName = dto.FileName ?? doc.FileName;
            doc.DocumentType = dto.DocumentType ?? doc.DocumentType;
            doc.FileUrl = dto.FileUrl ?? doc.FileUrl;
            doc.ExpiresAt = dto.ExpiresAt ?? doc.ExpiresAt;

            await _docs.UpdateAsync(doc);
            await _docs.SaveChangesAsync();

            return doc;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var doc = await _docs.GetByIdAsync(id);
            if (doc == null) return false;

            await _docs.DeleteAsync(doc);
            await _docs.SaveChangesAsync();

            return true;
        }
    }
}
