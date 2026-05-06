using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Costs
{
    public class UpdateOrderCostDto
    {
        public string? BillTo { get; set; }
        public string? Notes { get; set; }
        public decimal TaxRate { get; set; }
        public List<CostLineItemDto> LineItems { get; set; } = new();
    }
}
