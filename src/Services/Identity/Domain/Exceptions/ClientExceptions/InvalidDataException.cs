using LingoMq.Responses;
using Responses.StatusCodes;

namespace Identity.Domain.Exceptions.ClientExceptions;

public class InvalidDataException<T> : ExceptionBase
{
    public InvalidDataException() : base((int)ClientErrorCodes.BadRequest, "Invalid data") { }
    public InvalidDataException(string message)
        : base((int)ClientErrorCodes.BadRequest, message) { }
    public InvalidDataException(T data, string message = "Invalid data") :
        base(LingoMqResponse.BadRequestResult(data, message))
    { }
}
