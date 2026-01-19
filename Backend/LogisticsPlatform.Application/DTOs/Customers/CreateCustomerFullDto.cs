using LogisticsPlatform.Application.DTOs.Customers.Contacts;
using LogisticsPlatform.Application.DTOs.Customers.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Customers
{
    public class CreateCustomerFullDto
    {
        public CreateCustomerDto Customer { get; set; } = null!;
        public List<CreateCustomerAddressDto> Addresses { get; set; } = new();
        public List<CreateCustomerContactDto> Contacts { get; set; } = new();
        public List<CreateCustomerNoteDto> Notes { get; set; } = new();
    }

}
