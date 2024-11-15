namespace Reactivities.Domain.Core
{
    public class PagingParams
    {
        /// <remarks>
        /// Starting from 1.
        /// </remarks>
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public PagingParams(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            }
            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
