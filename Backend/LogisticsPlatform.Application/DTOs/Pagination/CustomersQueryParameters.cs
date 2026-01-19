using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Pagination
{
    public class CustomersQueryParameters : QueryParameters
    {
        public bool? IsActive { get; set; }
    }

}
