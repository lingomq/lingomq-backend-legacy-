using LingoMq.Responses;
using Responses;
using Responses.StatusCodes;

namespace Finances.BusinessLayer.Exceptions.ClientExceptions
{
    public class ForbiddenException<T> : ExceptionBase
    {
        public ForbiddenException() : base((int)ClientErrorCodes.Unauthorized,
            "access denied")
        { }
        public ForbiddenException(string message)
            : base((int)ClientErrorCodes.Unauthorized, message) { }
        public ForbiddenException(T data, string message = "access denied") :
            base(LingoMqResponse.ForbiddenResult(data, message))
        { }
    }
}
