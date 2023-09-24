namespace Authentication.BusinessLayer.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { } 
        public NotFoundException(string message = "Not found. ") : base(message) { }
    }
}
