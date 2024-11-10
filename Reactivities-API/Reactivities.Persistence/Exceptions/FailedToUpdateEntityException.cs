namespace Reactivities.Persistence.Exceptions
{
    internal class FailedToUpdateEntityException : ApplicationException
    {
        public FailedToUpdateEntityException(string? message) : base(message)
        {
        }
    }
}
