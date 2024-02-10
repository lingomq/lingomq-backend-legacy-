using LingoMqResponses;
using LingoMqResponses.StatusCodes;

namespace Finances.BusinessLayer.Exceptions.ClientExceptions
{
    public class ConflictException<T> : ExceptionBase
    {
        public ConflictException() : base((int)ClientErrorCodes.Conflict, "Received data was conflicted") { }
        public ConflictException(string message)
            : base((int)ClientErrorCodes.Conflict, message) { }
        public ConflictException(T data, string message = "Received data was conflicted") :
            base(LingoMqResponse.ConflictResult(data, message))
        { }
    }
}
