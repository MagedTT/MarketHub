namespace MarketHub.Application.Exceptions;

public abstract class BadRequestException : Exception
{
    public BadRequestException()
        : base()
    { }

    public BadRequestException(string message)
        : base(message)
    { }
}