using Microsoft.AspNetCore.Mvc;

namespace Authentication.BusinessLayer.Exceptions
{
    public class ClientExceptionBase : Exception
    {
        public int Code;
        public IActionResult? Result;
        public ClientExceptionBase() : base() { }
        public ClientExceptionBase(int code, string message) : base(message) => Code = code;
        public ClientExceptionBase(IActionResult? result) : base() => Result = result;
    }
}
