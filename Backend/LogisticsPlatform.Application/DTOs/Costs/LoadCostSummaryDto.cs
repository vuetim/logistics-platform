using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Costs
{
    public class LoadCostSummaryDto
    {
        public decimal CustomerRate { get; set; }
        public decimal CarrierRate { get; set; }
        public decimal Margin { get; set; }
        public decimal TotalBillable { get; set; }
        public decimal TotalPayable { get; set; }
    }

}
