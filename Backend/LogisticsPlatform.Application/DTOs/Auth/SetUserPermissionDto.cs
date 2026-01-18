using LogisticsPlatform.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Auth
{
    public class SetUserPermissionDto
    {
        public Permission Permission { get; set; }
        public bool? IsAllowed { get; set; } 
    }
}
