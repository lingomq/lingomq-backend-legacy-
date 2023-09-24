using Responses;

namespace Authentication.BusinessLayer.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() => 
            StatusCode.NotFoundResult("Object not found. See error for additional info");
        public NotFoundException(string message = "Not found. ") : base(message) =>
            StatusCode.NotFoundResult(message);
    }
}
