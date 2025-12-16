using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Loads
{
    public class DispatchLoadDto
    {
        public string DriverName { get; set; } = string.Empty;
        public string? DriverPhone { get; set; }
        public string? DriverEmail { get; set; }

        public string TruckNumber { get; set; } = string.Empty;
        public string? TrailerNumber { get; set; }
    }

}
