namespace MarketHub.Application.Exceptions;

public class InventoryNotFoundException : NotFoundException
{
    public InventoryNotFoundException(string message)
        : base(message)
    { }
}