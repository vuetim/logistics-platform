using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs
{
    public class AssignDelayResponsibilityDto
    {
        public DelayResponsibilityType Responsibility { get; set; }
        public string? Reason { get; set; }
    }
}
