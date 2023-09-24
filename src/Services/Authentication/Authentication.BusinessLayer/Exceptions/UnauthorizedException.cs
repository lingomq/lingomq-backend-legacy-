using LingoMq.Responses;
using Microsoft.AspNetCore.Mvc;
using Responses.StatusCodes;

namespace Authentication.BusinessLayer.Exceptions
{
    public class UnauthorizedException<T> : ClientExceptionBase
    {
        public UnauthorizedException() : base((int)ClientErrorCodes.Unauthorized,
            "You aren't authorize") { }
        public UnauthorizedException(string message)
            : base((int)ClientErrorCodes.Unauthorized, message) { }
        public UnauthorizedException(T data, string message = "You aren't authorize") :
            base(StatusCode.UnauthorizedResult(data, message)) { }
    }
}
