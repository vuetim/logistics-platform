using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Costs
{
    public class UpdateLoadCostDto
    {
        public string? Notes { get; set; }
        public List<CostLineItemDto> LineItems { get; set; } = new();
    }
}
