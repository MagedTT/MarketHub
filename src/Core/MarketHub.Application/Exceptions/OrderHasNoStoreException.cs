namespace MarketHub.Application.Exceptions;

public class OrderHasNoStoreException : BadRequestException
{
    public OrderHasNoStoreException(string message)
        : base(message)
    { }
}