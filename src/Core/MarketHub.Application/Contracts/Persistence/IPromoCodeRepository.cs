using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IPromoCodeRepository
{
    Task<PromoCode?> PromoCodeExistsByCodeAsync(string code);
}