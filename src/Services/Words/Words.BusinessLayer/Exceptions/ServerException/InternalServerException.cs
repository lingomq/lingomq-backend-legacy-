using LingoMq.Responses;

namespace Words.BusinessLayer.Exceptions.ServerException
{
    public class InternalServerException : ExceptionBase
    {
        public InternalServerException() : base(0,
            "Something wrong!")
        { }
        public InternalServerException(string message = "Something wrong") :
            base(StatusCode.InternalServerError(message))
        { }
    }
}
