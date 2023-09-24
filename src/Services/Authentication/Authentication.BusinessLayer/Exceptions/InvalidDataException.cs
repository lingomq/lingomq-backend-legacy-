using LingoMq.Responses;
using Responses.StatusCodes;

namespace Authentication.BusinessLayer.Exceptions
{
    public class InvalidDataException<T> : ClientExceptionBase
    {
        public InvalidDataException() : base((int)ClientErrorCodes.BadRequest, "Invalid data") { }
        public InvalidDataException(string message)
            : base((int)ClientErrorCodes.BadRequest, message) { }
        public InvalidDataException(T data, string message = "Invalid data") :
            base(StatusCode.BadRequestResult(data, message)) { }
    }
}
