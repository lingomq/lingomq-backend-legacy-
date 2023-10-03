using LingoMq.Responses;
using Responses.StatusCodes;

namespace Words.BusinessLayer.Exceptions.ClientExceptions
{
    public class ForbiddenException<T> : ExceptionBase
    {
        public ForbiddenException() : base((int)ClientErrorCodes.Unauthorized,
            "access denied")
        { }
        public ForbiddenException(string message)
            : base((int)ClientErrorCodes.Unauthorized, message) { }
        public ForbiddenException(T data, string message = "access denied") :
            base(StatusCode.ForbiddenResult(data, message))
        { }
    }
}
