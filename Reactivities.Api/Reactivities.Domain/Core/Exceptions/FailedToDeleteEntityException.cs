namespace Reactivities.Domain.Core.Exceptions
{
    public class FailedToDeleteEntityException : ApplicationException
    {
        public FailedToDeleteEntityException(string? message) : base(message)
        {
        }
    }
}
