namespace LogisticsPlatform.Application.DTOs.Orders.Notes
{
    public class OrderNoteDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
