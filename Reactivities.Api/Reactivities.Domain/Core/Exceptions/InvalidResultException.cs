namespace Reactivities.Domain.Core.Exceptions
{
    internal sealed class InvalidResultException : ApplicationException
    {
        public InvalidResultException() : base("Unknown error")
        {
        }

        public InvalidResultException(string? message) : base(message)
        {
        }

        public InvalidResultException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
