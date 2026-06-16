namespace MarketHub.Application.Exceptions;

public abstract class NotFoundException : Exception
{
    public NotFoundException()
        : base()
    { }

    public NotFoundException(string message)
        : base(message)
    { }
}