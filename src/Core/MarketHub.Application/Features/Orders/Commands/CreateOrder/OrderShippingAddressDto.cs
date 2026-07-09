namespace MarketHub.Application.Features.Orders.Commands.CreateOrder;

public class OrderShippingAddressDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string BuildingNumber { get; set; } = string.Empty;
    public string? Floor { get; set; }
    public string? Apartment { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}