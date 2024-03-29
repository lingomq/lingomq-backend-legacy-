﻿using LingoMq.Responses;

namespace Words.Domain.Exceptions.ServerException;

public class InternalServerException : ExceptionBase
{
    public InternalServerException() : base(0,
        "Something wrong!")
    { }
    public InternalServerException(string message = "Something wrong") :
        base(LingoMqResponse.InternalServerError(message))
    { }
}
