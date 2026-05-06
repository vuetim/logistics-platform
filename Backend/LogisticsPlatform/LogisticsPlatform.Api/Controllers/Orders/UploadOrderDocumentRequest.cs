using LogisticsPlatform.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.Api.Controllers.Orders
{
    public class UploadOrderDocumentRequest
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; } = null!;

        [FromForm(Name = "documentType")]
        public OrderDocumentType DocumentType { get; set; } = OrderDocumentType.Other;

        [FromForm(Name = "isInternal")]
        public bool IsInternal { get; set; }

        [FromForm(Name = "copyToLoad")]
        public bool CopyToLoad { get; set; } = true;
    }
}
