using LingoMq.Responses;

namespace Authentication.BusinessLayer.Exceptions.ServerExceptions
{
    public class InternalServerException : ExceptionBase
    {
        public InternalServerException() : base (0,
            "Something wrong!") { }
        public InternalServerException(string message = "Something wrong") :
            base(StatusCode.InternalServerError(message))
        { }
    }
}
