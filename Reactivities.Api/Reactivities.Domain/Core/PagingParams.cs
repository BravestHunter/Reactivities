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
            ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
