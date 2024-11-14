namespace Reactivities.Domain.Core.Exceptions
{
    public class FailedToUpdateEntityException : ApplicationException
    {
        public FailedToUpdateEntityException(string? message) : base(message)
        {
        }
    }
}
