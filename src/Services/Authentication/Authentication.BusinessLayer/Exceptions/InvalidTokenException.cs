using LingoMq.Responses;
using Microsoft.AspNetCore.Mvc;
using Responses.StatusCodes;

namespace Authentication.BusinessLayer.Exceptions
{
    public class InvalidTokenException<T> : ClientExceptionBase
    {
        public InvalidTokenException() : base((int) ClientErrorCodes.Forbidden, 
            "Invalid token") { }
        public InvalidTokenException(string message)
            : base((int)ClientErrorCodes.Forbidden, message) { }
        public InvalidTokenException(T data, string message = "Invalid token") :
            base(StatusCode.ForbiddenResult(data, message)) { }
    }
}
