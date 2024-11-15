namespace Reactivities.Domain.Core
{
    public class PagedList<T>
    {
        public List<T> Items { get; init; }
        public PageParams Params { get; init; }

        public PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = new List<T>(items);
            Params = new PageParams()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                TotalItems = totalItems
            };
        }
    }
}
