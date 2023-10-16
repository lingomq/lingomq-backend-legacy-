using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Authentication.BusinessLayer.Exceptions
{
    /// <summary>
    /// This class (<c>ExceptionBase</c>) is a custom parent exceptions
    /// for work with responses
    /// </summary>
    public class ExceptionBase : Exception
    {
        public int Code;
        public IActionResult? Result;
        public HttpStatusCode ExceptionStatusCode { get; set; } = HttpStatusCode.BadRequest;
        public ExceptionBase() : base() { }
        public ExceptionBase(int code, string message) : base(message) => Code = code;
        public ExceptionBase(IActionResult? result) : base() => Result = result;
    }
}
