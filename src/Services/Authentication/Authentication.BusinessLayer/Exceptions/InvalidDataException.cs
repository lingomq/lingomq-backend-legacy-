using LingoMq.Responses;

namespace Authentication.BusinessLayer.Exceptions
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException() : base() => StatusCode.BadRequestResult("Invalid data");
        public InvalidDataException(string message = "Invalid data") : base(message) { }
    }
}
