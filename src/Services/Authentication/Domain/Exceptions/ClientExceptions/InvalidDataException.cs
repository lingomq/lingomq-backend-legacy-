﻿using LingoMqResponses;
using LingoMqResponses.StatusCodes;

namespace Authentication.Domain.Exceptions
{
    public class InvalidDataException<T> : ExceptionBase
    {
        public InvalidDataException() : base((int)ClientErrorCodes.BadRequest, "Invalid data") { }
        public InvalidDataException(string message)
            : base((int)ClientErrorCodes.BadRequest, message) { }
        public InvalidDataException(T data, string message = "Invalid data") :
            base(LingoMqResponse.BadRequestResult(data, message))
        { }
    }
}
