namespace Biletall.Core.Utilities.Results
{
    public interface IResult : IResult<string>
    {

    }

    public interface IResult<T>
    {
        T Data { get; set; }
        bool IsSuccess { get; }
        string Message { get; }
    }
}
