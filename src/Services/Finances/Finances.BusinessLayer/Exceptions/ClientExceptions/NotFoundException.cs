﻿using LingoMq.Responses;
using Responses;
using Responses.StatusCodes;

namespace Finances.BusinessLayer.Exceptions.ClientExceptions
{
    public class NotFoundException<T> : ExceptionBase
    {
        public NotFoundException() : base((int)ClientErrorCodes.NotFound, "Not found") { }
        public NotFoundException(string message)
            : base((int)ClientErrorCodes.NotFound, message) { }
        public NotFoundException(T data, string message = "Not found") :
            base(LingoMqResponse.NotFoundResult(data, message))
        { }
    }
}
