namespace LogisticsPlatform.Application.DTOs.Orders.Notes
{
    public class CreateOrderNoteDto
    {
        public string Message { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
    }
}
