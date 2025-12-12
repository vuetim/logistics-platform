using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Financial
{
    public class CreateSettlementDto
    {
        public DateTime? DueDate { get; set; }

        public DateTime SettlementDate { get; set; }
        public string? Notes { get; set; }
    }


}
