namespace Reactivities.Domain.Core
{
    public class PageParams
    {
        /// <remarks>
        /// Starting from 1.
        /// </remarks>
        public required int CurrentPage { get; set; }
        public required int PageSize { get; set; }
        public required int TotalPages { get; set; }
        public required int TotalItems { get; set; }
    }
}
