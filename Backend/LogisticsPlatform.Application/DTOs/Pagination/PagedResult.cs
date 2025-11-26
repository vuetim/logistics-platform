namespace LogisticsPlatform.Application.DTOs.Pagination
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PagedResult() { }

        public PagedResult(IEnumerable<T> items, int total, int page, int pageSize)
        {
            Items = items;
            TotalCount = total;
            Page = page;
            PageSize = pageSize;
        }
    }
}
