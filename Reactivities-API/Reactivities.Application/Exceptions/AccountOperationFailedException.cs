﻿namespace Reactivities.Application.Exceptions
{
    internal class AccountOperationFailedException : ApplicationException
    {
        public AccountOperationFailedException(string? message) : base(message)
        {
        }

        public AccountOperationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
