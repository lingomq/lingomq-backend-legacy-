using LingoMq.Responses;

namespace Authentication.BusinessLayer.Exceptions
{
    public class UnauthorizedException<T> : Exception
    {
        public UnauthorizedException() : base() => StatusCode.UnauthorizedResult();
        public UnauthorizedException(T data) : base() =>
            StatusCode.UnauthorizedResult(data, "Received data was conflicted");
    }
}
