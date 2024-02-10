﻿using LingoMqResponses;
using LingoMqResponses.StatusCodes;

namespace Topics.Domain.Exceptions.ClientExceptions
{
    public class UnauthorizedException<T> : ExceptionBase
    {
        public UnauthorizedException() : base((int)ClientErrorCodes.Unauthorized,
            "You aren't authorize")
        { }
        public UnauthorizedException(string message)
            : base((int)ClientErrorCodes.Unauthorized, message) { }
        public UnauthorizedException(T data, string message = "You aren't authorize") :
            base(LingoMqResponse.UnauthorizedResult(data, message))
        { }
    }
}
