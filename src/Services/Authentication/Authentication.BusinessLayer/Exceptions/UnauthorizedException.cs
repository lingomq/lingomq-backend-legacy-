namespace Authentication.BusinessLayer.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base() { }
        public UnauthorizedException(string message = "Access denied. ") : base(message) { }
    }
}
