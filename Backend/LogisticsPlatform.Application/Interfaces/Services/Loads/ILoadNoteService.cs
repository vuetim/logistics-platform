using LogisticsPlatform.Application.DTOs.Loads.LoadNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads
{
    public interface ILoadNoteService
    {
        Task AddAsync(Guid loadId, CreateLoadNoteDto dto, Guid userId);
        Task<List<LoadNoteDto>> GetByLoadAsync(Guid loadId, Guid userId);
    }

}
