using Reactivities.Domain.Core.Exceptions;

namespace Reactivities.Domain.Core
{
#pragma warning disable CA1000
    public readonly struct Result
    {
        private static readonly Exception DefaultException = new InvalidResultException();

        private readonly bool _success;
        private readonly Exception _exception;

        public bool IsSuccess => _success;
        public bool IsFailure => !_success;
        public Exception Exception => _exception;

        private Result(bool success, Exception? exception = null)
        {
            _success = success;
            _exception = exception ?? DefaultException;
        }

        public void ThrowIfError()
        {
            if (IsFailure) throw _exception;
        }

        public static Result Success()
        {
            return new Result(true);
        }

        public static Result Failure(Exception exception)
        {
            return new Result(false, exception: exception);
        }

        public static Result Failure(string errorMessage)
        {
            return new Result(false, exception: new InvalidResultException(errorMessage));
        }

        public static Result Failure(string errorMessage, Exception exception)
        {
            return new Result(false, exception: new InvalidResultException(errorMessage, exception));
        }

        public static Result Failure()
        {
            return new Result(false);
        }
    }

    public readonly struct Result<T>
    {
        private static readonly Exception DefaultException = new InvalidResultException();

        private readonly bool _success;
        private readonly T? _value;
        private readonly Exception _exception;

        public bool IsSuccess => _success;
        public bool IsFailure => !_success;
        public T? Value => _value;
        public Exception Exception => _exception;

        private Result(bool success, T? value = default, Exception? exception = null)
        {
            _success = success;
            _value = value;
            _exception = exception ?? DefaultException;
        }

        public T GetOrThrow() => _value ?? throw _exception;

        public void ThrowIfError()
        {
            if (IsFailure) throw _exception;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value: value);
        }

        public static Result<T> Failure(Exception exception)
        {
            return new Result<T>(false, exception: exception);
        }

        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>(false, exception: new InvalidResultException(errorMessage));
        }

        public static Result<T> Failure(string errorMessage, Exception exception)
        {
            return new Result<T>(false, exception: new InvalidResultException(errorMessage, exception));
        }

        public static Result<T> Failure()
        {
            return new Result<T>(false);
        }
    }
#pragma warning restore CA1000
}
