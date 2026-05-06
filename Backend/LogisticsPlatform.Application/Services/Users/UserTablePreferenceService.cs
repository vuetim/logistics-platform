using LogisticsPlatform.Application.DTOs.Common;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services.Users;
using LogisticsPlatform.Domain.Entities.Security;

namespace LogisticsPlatform.Application.Services.Users
{
    public class UserTablePreferenceService : IUserTablePreferenceService
    {
        private readonly IUserTablePreferenceRepository _repo;

        public UserTablePreferenceService(IUserTablePreferenceRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserTablePreferenceDto> GetAsync(Guid userId, string tableKey)
        {
            var normalizedKey = NormalizeKey(tableKey);
            var pref = await _repo.GetAsync(userId, normalizedKey);

            return new UserTablePreferenceDto
            {
                TableKey = normalizedKey,
                JsonConfig = pref?.JsonConfig ?? "{}",
                UpdatedAt = pref?.UpdatedAt ?? pref?.CreatedAt ?? DateTime.UtcNow
            };
        }

        public async Task<UserTablePreferenceDto> UpsertAsync(Guid userId, string tableKey, UpdateUserTablePreferenceDto dto)
        {
            var normalizedKey = NormalizeKey(tableKey);
            var jsonConfig = string.IsNullOrWhiteSpace(dto.JsonConfig) ? "{}" : dto.JsonConfig.Trim();

            var pref = await _repo.GetAsync(userId, normalizedKey);
            if (pref == null)
            {
                pref = new UserTablePreference
                {
                    UserId = userId,
                    TableKey = normalizedKey,
                    JsonConfig = jsonConfig
                };
                await _repo.AddAsync(pref);
            }
            else
            {
                pref.JsonConfig = jsonConfig;
                pref.UpdatedAt = DateTime.UtcNow;
                _repo.Update(pref);
            }

            await _repo.SaveChangesAsync();

            return new UserTablePreferenceDto
            {
                TableKey = pref.TableKey,
                JsonConfig = pref.JsonConfig,
                UpdatedAt = pref.UpdatedAt ?? pref.CreatedAt
            };
        }

        private static string NormalizeKey(string tableKey)
        {
            var value = (tableKey ?? string.Empty).Trim().ToLowerInvariant();
            return string.IsNullOrWhiteSpace(value) ? "orders" : value;
        }
    }
}
