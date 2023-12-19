using LingoMq.Responses;
using Responses.StatusCodes;

namespace Notifications.Domain.Exceptions.ClientExceptions
{
    public class InvalidDataException<T> : ExceptionBase
    {
        public InvalidDataException() : base((int)ClientErrorCodes.BadRequest, "Invalid data") { }
        public InvalidDataException(string message)
            : base((int)ClientErrorCodes.BadRequest, message) { }
        public InvalidDataException(T data, string message = "Invalid data") :
            base(LingoMqResponse.BadRequestResult(data, message))
        { }
        public InvalidDataException(T data, string[] parameters) :
            base(LingoMqResponse.BadRequestResult(data, "Some parameters is incorrect: " + string.Join(";", parameters)))
        { }

        public InvalidDataException(string[] parameters) :
            base(LingoMqResponse.BadRequestResult("Some parameters is incorrect: " + string.Join(";", parameters)))
        { }
    }
}
