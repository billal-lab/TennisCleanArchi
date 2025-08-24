using System.Net;

namespace TennisCleanArchi.Application.Common.Exceptions;
public class CustomException : Exception
{
    public List<string>? ErrorMessages { get; }

    public HttpStatusCode StatusCode { get; }

    public string? ErrorCode { get; set; }

    public CustomException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? errorCode = null)
        : base(message)
    {
        ErrorMessages = errors;
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}
