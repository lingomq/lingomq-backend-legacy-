namespace Authentication.BusinessLayer.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() { }
        public InvalidTokenException(string message = "Invalid token.") : base(message) { }
        public InvalidTokenException(Exception innerException, string message = "Invalid token.") 
            : base(message, innerException) { }
    }
}
