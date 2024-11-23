namespace Reactivities.Domain.Core.Exceptions
{
    public class MissingConfigurationException : ApplicationException
    {
        public MissingConfigurationException(string? message) : base(message)
        {
        }
    }
}
