﻿using LingoMq.Responses;
using Responses;

namespace Finances.BusinessLayer.Exceptions.ServerExceptions
{
    public class InternalServerException : ExceptionBase
    {
        public InternalServerException() : base(0,
            "Something wrong!")
        { }
        public InternalServerException(string message = "Something wrong") :
            base(LingoMqResponse.InternalServerError(message))
        { }
    }
}
