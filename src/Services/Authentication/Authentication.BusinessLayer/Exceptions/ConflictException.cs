namespace Authentication.BusinessLayer.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() : base() { }
        public ConflictException(string message = "Received data was conflicted") : base(message) { }
    }
}
