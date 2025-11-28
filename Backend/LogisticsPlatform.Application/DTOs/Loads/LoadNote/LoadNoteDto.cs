using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Loads.LoadNote
{
    public class LoadNoteDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsInternal { get; set; }
    }


}
