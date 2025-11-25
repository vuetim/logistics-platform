namespace LogisticsPlatform.Application.DTOs.Carriers.Notes
{
    public class CreateCarrierNoteDto
    {
        public Guid CarrierId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
