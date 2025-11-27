namespace LogisticsPlatform.Application.DTOs.Pagination
{
    public class CarrierQueryParameters : QueryParameters
    {
        public string? Status { get; set; } // Active, Inactive, Blocked
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
    }
}
