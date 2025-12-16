using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs
{
    public class TenderCarrierDto
    {
        public Guid LoadId { get; set; }
        public Guid CarrierId { get; set; }

        public decimal? OfferedRate { get; set; }
        public string? Currency { get; set; }
    }

}
