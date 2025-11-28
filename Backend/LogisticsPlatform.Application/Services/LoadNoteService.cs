using LogisticsPlatform.Application.DTOs.Loads.LoadNote;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Services
{
    public class LoadNoteService : ILoadNoteService
    {
        private readonly ILoadNoteRepository _repo;

        public LoadNoteService(ILoadNoteRepository repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(Guid loadId, CreateLoadNoteDto dto, Guid userId)
        {
            var note = new LoadNote
            {
                LoadId = loadId,
                Message = dto.Message,
                IsInternal = dto.IsInternal,
                CreatedByUserId = userId
            };

            await _repo.AddAsync(note);
        }

        public async Task<List<LoadNoteDto>> GetByLoadAsync(Guid loadId)
        {
            var notes = await _repo.GetByLoadIdAsync(loadId);

            return notes.Select(n => new LoadNoteDto
            {
                Id = n.Id,
                Message = n.Message,
                CreatedByName = n.CreatedByUser.FullName,
                CreatedAt = n.CreatedAt,
                IsInternal = n.IsInternal
            }).ToList();
        }
    }

}
