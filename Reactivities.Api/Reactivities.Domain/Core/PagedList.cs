namespace Reactivities.Domain.Core
{
    public class PagedList<T>
    {
        public List<T> Items { get; init; }
        public int CurrentPage { get; init; }
        public int TotalPages { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }

        public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = new List<T>(items);
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
