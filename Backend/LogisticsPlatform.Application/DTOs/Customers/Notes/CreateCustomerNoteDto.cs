namespace LogisticsPlatform.Application.DTOs.Customers.Notes
{
    public class CreateCustomerNoteDto
    {
        public Guid CustomerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
