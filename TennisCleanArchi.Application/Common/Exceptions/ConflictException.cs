namespace TennisCleanArchi.Application.Common.Exceptions;

public class ConflictException : CustomException
{
    public ConflictException(string message)
        : base(message, null, System.Net.HttpStatusCode.Conflict)
    {
    }
    public ConflictException(string message, string errorCode)
      : base(message, null, System.Net.HttpStatusCode.Conflict, errorCode)
    {
    }
}
