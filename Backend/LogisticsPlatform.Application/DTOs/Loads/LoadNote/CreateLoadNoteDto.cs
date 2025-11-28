using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Loads.LoadNote
{
    public class CreateLoadNoteDto
    {
        public string Message { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
    }
}
