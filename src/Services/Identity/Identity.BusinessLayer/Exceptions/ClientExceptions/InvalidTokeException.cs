﻿using LingoMq.Responses;
using Responses.StatusCodes;

namespace Identity.BusinessLayer.Exceptions.ClientExceptions
{
    public class InvalidTokeException<T> : ExceptionBase
    {
        public InvalidTokenException() : base((int)ClientErrorCodes.Forbidden,
            "Invalid token")
        { }
        public InvalidTokenException(string message)
            : base((int)ClientErrorCodes.Forbidden, message) { }
        public InvalidTokenException(T data, string message = "Invalid token") :
            base(StatusCode.ForbiddenResult(data, message))
        { }
    }
}
