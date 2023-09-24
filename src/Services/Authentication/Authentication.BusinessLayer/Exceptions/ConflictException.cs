using LingoMq.Responses;

namespace Authentication.BusinessLayer.Exceptions
{
    public class ConflictException<T> : Exception
    {
        public ConflictException() : base() => StatusCode.ConflictResult("Received data was conflicted");
        public ConflictException(T data) : base() =>
            StatusCode.ConflictResult(data, "Received data was conflicted");
    }
}
