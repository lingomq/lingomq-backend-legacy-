namespace Authentication.BusinessLayer.Exceptions
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException() : base() { }
        public InvalidDataException(string message = "Invalid data") : base(message) { }
    }
}
