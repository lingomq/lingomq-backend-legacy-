using LingoMq.Responses;
using Responses.StatusCodes;

namespace Authentication.BusinessLayer.Exceptions
{
    public class ConflictException<T> : ExceptionBase
    {
        public ConflictException() : base((int)ClientErrorCodes.Conflict, "Received data was conflicted") { }
        public ConflictException(string message) 
            : base((int)ClientErrorCodes.Conflict, message) { }
        public ConflictException(T data, string message = "Received data was conflicted") :
            base(StatusCode.ConflictResult(data, message)) { }
    }
}
