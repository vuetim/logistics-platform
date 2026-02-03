

using LogisticsPlatform.Domain.Entities;

namespace LogisticsPlatform.Application.DTOs.Customers.Notes
{
    public class CustomerNoteDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public Guid CreatedByUserId { get; set; }
        public string CreatedByName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }



}
