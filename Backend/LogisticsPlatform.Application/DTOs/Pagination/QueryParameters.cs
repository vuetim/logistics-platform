namespace LogisticsPlatform.Application.DTOs.Pagination
{
    public class QueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public string? SortBy { get; set; }
        public string SortDir { get; set; } = "asc";

        public Dictionary<string, string>? Filters { get; set; }
    }
}
