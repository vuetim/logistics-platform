using LogisticsPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Loads.LoadDocuments
{
    public class LoadDocumentDto
    {
        public Guid Id { get; set; }
        public LoadDocumentType DocumentType { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsInternal { get; internal set; }
    }
}
