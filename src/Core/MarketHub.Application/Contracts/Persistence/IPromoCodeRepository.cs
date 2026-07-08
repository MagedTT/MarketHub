using MarketHub.Application.DTOs.Persistence.PromoCodes;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Contracts.Persistence;

public interface IPromoCodeRepository
{
    Task<PromoCode?> PromoCodeExistsByCodeAsync(string code);

    Task<PromoCode?> GetByIdAsync(Guid promoCodeId);
    Task<PromoCode?> GetByCodeAsync(string code);
    Task<PagedList<PromoCodeDto>> GetAllPromoCodesAsync(PromoCodeParameters promoCodeParameters, bool trackChanges);
    Task<int> GetUsageCountByIdAsync(Guid promoCodeId);
    Task<int> GetUsageCountByCodeAsync(string code);
    Task<bool> CheckPromoCodeExistsByIdAsync(Guid promoCodeId);
    Task<bool> CheckPromoCodeExistsByCodeAsync(string code);
    Task<int> TotalPromoCodesByStoreIdAsync(Guid storeId);
    Task<bool> CheckPromoCodeUniqueByCodeAsync(string code);
    void CreatePromoCode(PromoCode promoCode);
    void UpdatePromoCode(PromoCode promoCode);
    void DeletePromoCode(PromoCode promoCode);
}