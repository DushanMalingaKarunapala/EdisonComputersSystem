using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    //a class to represent the result of an operation, which can be extended in the future to include success status, error messages, etc.
    public class Result<T> where T : class
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public List<string> Errors { get; private set; } = new();
        public Exception? Exception { get; private set; }

        private Result() { }

        // Success factory methods
        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static Result<T> Success()
        {
            return new Result<T>
            {
                IsSuccess = true
            };
        }

        // Failure factory methods
        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }

        public static Result<T> Failure(List<string> errors)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Errors = errors,
                ErrorMessage = string.Join(", ", errors)
            };
        }
    }


    // Non-generic version for operations that don't return data
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public List<string> Errors { get; private set; } = new();
        public Exception? Exception { get; private set; }

        private Result() { }

        public static Result Success()
        {
            return new Result { IsSuccess = true };
        }

        public static Result Failure(string errorMessage)
        {
            return new Result
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }

        public static Result Failure(List<string> errors)
        {
            return new Result
            {
                IsSuccess = false,
                Errors = errors,
                ErrorMessage = string.Join(", ", errors)
            };
        }

        public static Result Failure(Exception exception)
        {
            return new Result
            {
                IsSuccess = false,
                Exception = exception,
                ErrorMessage = exception.Message
            };
        }
    }

}
