using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.CommonResult
{
    public class Result
    {
        protected readonly List<Error> _errors = [];
        public IReadOnlyList<Error> Errors => _errors.AsReadOnly();
        public bool IsSuccess => _errors.Count == 0;
        public bool IsFailure => !IsSuccess;
        protected Result() { }
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }
        protected Result(Error error)
        {
            _errors.Add(error);
        }

        public static Result Ok()
        {
            return new Result();
        }
        public static Result Fail(List<Error> errors)
        {
            return new Result(errors);
        }
        public static Result Fail(Error error)
        {
            return new Result(error);
        }

    }

    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");

        private Result(T value)
        {
            _value = value;
        }
        private Result(Error error) : base(error)
        {
            _value = default!;
        }

        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        public static Result<T> Ok(T value)=>new Result<T>(value);
        public static new Result<T> Fail(List<Error> errors) => new Result<T>(errors);
        public static new Result<T> Fail(Error error) => new Result<T>(error);

        public static implicit operator Result<T>(T value) => Ok(value);
        public static implicit operator Result<T>(Error error) => Fail(error);
        public static implicit operator Result<T>(List<Error> errors) => Fail(errors);
    }
}
