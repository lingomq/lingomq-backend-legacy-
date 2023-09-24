using LingoMq.Responses;

namespace Authentication.BusinessLayer.Exceptions
{
    public class InvalidTokenException<T> : Exception
    {
        public InvalidTokenException() : base() => StatusCode.ForbiddenResult("Invalid token");
        public InvalidTokenException(T data) : base() => 
            StatusCode.ForbiddenResult(data, "Received data was conflicted");
    }
}
