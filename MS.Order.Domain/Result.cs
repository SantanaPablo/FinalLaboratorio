using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOrder.Domain
{
    /// <summary>
    /// Implementation of the Result pattern for handling operation outcomes.
    /// </summary>
    public class Result
    {
        internal Result(bool isSuccess, string error)
        {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException();
            if (!isSuccess && error == string.Empty)
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public string Error { get; }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Fail(string message) => new Result(false, message);
        public static Result<T> Success<T>(T value) => new Result<T>(value, true, string.Empty);
        public static Result<T> Fail<T>(string message) => new Result<T>(default, false, message);
    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        internal Result(T? value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        [NotNull]
        public T Value => _value! ?? throw new InvalidOperationException("Result has no value");
    }
}