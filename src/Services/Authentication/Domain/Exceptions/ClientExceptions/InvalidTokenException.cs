﻿using LingoMqResponses;
using LingoMqResponses.StatusCodes;

namespace Authentication.Domain.Exceptions
{
    public class InvalidTokenException<T> : ExceptionBase
    {
        public InvalidTokenException() : base((int)ClientErrorCodes.Forbidden,
            "Invalid token")
        { }
        public InvalidTokenException(string message)
            : base((int)ClientErrorCodes.Forbidden, message) { }
        public InvalidTokenException(T data, string message = "Invalid token") :
            base(LingoMqResponse.ForbiddenResult(data, message))
        { }
    }
}
