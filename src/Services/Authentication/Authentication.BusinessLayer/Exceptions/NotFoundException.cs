using LingoMq.Responses;
using Responses.StatusCodes;

namespace Authentication.BusinessLayer.Exceptions
{
    public class NotFoundException<T> : ClientExceptionBase
    {
        public NotFoundException() : base((int)ClientErrorCodes.NotFound, "Not found") { }
        public NotFoundException(string message)
            : base((int)ClientErrorCodes.NotFound, message) { }
        public NotFoundException(T data, string message = "Not found") :
            base(StatusCode.NotFoundResult(data, message)) { }
    }
}
