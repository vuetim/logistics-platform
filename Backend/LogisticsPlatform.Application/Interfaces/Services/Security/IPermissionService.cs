using LogisticsPlatform.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services.Security
{
    public interface IPermissionService
    {
        Task<HashSet<Permission>> GetEffectivePermissionsAsync(Guid userId);
        Task<bool> HasPermissionAsync(Guid userId,
        Permission permission);

    }

}
