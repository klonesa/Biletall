using Biletall.Core.Constants;
using System.Net;

namespace Biletall.Core.Utilities.Results
{
    public class Result : Result<string>, IResult
    {
        public Result() : base() { }

        public Result(string data)
        {
            Data = data;
        }

        public Result(bool success) : base(success)
        {
        }
        public Result(bool success, string message) : base(success, message)
        {
        }
        public Result(bool success, string message, string data) : base(success, message, data)
        {
        }
    }

    public class Result<T> : IResult<T>
    {
        public Result() { }

        public Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public Result(T data)
        {
            Data = data;
        }

        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }
        public Result(bool success, string message, T data) : this(success, message)
        {
            Data = data;
        }

        public T Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = ResultMessages.Successful;
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
    }
}
