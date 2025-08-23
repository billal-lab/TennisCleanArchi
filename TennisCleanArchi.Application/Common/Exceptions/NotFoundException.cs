using System.Net;
using TennisCleanArchi.Application.Common.Exceptions;

namespace TennisCleanArchi.Application.Common.Exceptions;
public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message, null, HttpStatusCode.NotFound)
    {
    }

    public NotFoundException(string message, string errorCode)
      : base(message, null, HttpStatusCode.NotFound, errorCode)
    {
    }
}
