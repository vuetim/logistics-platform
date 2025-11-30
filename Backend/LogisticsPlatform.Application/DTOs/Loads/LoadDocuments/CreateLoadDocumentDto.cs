using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Loads.LoadDocuments
{
    public class CreateLoadDocumentDto
    {
        public LoadDocumentType DocumentType { get; set; } = LoadDocumentType.POD;
        public string FileUrl { get; set; } = string.Empty;
        public bool IsInternal { get;  set; }
    }
}
