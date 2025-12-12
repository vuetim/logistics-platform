using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Financial
{
    public class UpdateInvoiceStatusDto
    {
        public InvoiceStatus Status { get; set; }
    }

}
