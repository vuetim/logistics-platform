using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public Permission Permission { get; set; }
        public bool IsAllowed { get; set; } = true;
    }
}
