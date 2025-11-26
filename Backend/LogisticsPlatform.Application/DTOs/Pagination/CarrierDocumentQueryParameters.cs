using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Pagination
{
    public class CarrierDocumentQueryParameters : QueryParameters
    {
        public Guid? CarrierId { get; set; }       // filter by carrier
        public string? DocumentType { get; set; }  // filter by type
        public DateTime? ExpiringBefore { get; set; }
    }
}
