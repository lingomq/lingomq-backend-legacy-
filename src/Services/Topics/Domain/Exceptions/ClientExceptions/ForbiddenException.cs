﻿using LingoMqResponses;
using LingoMqResponses.StatusCodes;

namespace Topics.Domain.Exceptions.ClientExceptions
{
    public class ForbiddenException<T> : ExceptionBase
    {
        public ForbiddenException() : base((int)ClientErrorCodes.Unauthorized,
            "access denied")
        { }
        public ForbiddenException(string message)
            : base((int)ClientErrorCodes.Unauthorized, message) { }
        public ForbiddenException(T data, string message = "access denied") :
            base(LingoMqResponse.ForbiddenResult(data, message))
        { }
    }
}
