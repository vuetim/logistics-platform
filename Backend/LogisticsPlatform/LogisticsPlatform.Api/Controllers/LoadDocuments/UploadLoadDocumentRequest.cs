using LogisticsPlatform.Domain.Enums;

namespace LogisticsPlatform.Api.Controllers.LoadDocuments
{
    public class UploadLoadDocumentRequest
    {
        public LoadDocumentType DocumentType { get; set; }
        public IFormFile File { get; set; } = null!;
        public bool IsInternal { get; internal set; }
    }
}
