using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Financial
{
    public class CreateInvoiceDto
    {
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Notes { get; set; }
    }


}
