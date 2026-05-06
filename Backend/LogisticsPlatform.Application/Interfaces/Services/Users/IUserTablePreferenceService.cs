using LogisticsPlatform.Application.DTOs.Common;

namespace LogisticsPlatform.Application.Interfaces.Services.Users
{
    public interface IUserTablePreferenceService
    {
        Task<UserTablePreferenceDto> GetAsync(Guid userId, string tableKey);
        Task<UserTablePreferenceDto> UpsertAsync(Guid userId, string tableKey, UpdateUserTablePreferenceDto dto);
    }
}
