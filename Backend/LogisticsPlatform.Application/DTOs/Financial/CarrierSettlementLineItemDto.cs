using LogisticsPlatform.Domain.Entities.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Financial
{
    public class CarrierSettlementLineItemDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SettlementId { get; set; }
        public CarrierSettlement Settlement { get; set; } = null!;

        public string Description { get; set; } = string.Empty;

        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }

        public bool Billable { get; set; } = true;
        public string? Notes { get; set; }
    }
}
