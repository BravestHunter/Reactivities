﻿namespace Reactivities.Domain.Core.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string? message) : base(message)
        {
        }
    }
}
