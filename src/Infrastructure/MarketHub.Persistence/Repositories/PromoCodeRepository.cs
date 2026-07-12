using MarketHub.Application.Contracts.Persistence;
using MarketHub.Application.DTOs.Persistence.PromoCodes;
using MarketHub.Application.Shared;
using MarketHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class PromoCodeRepository : IPromoCodeRepository
{
    private readonly MarketHubDbContext _context;
    public PromoCodeRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<PromoCode?> GetByIdAsync(Guid promoCodeId)
        => await _context.PromoCodes.FirstOrDefaultAsync(x => x.Id == promoCodeId);

    public async Task<PromoCode?> GetByCodeAsync(string code)
        => await _context.PromoCodes.FirstOrDefaultAsync(x => x.Code == code);

    public async Task<PagedList<PromoCodeDto>> GetAllPromoCodesAsync(PromoCodeParameters promoCodeParameters, bool trackChanges)
    {
        IQueryable<PromoCode> promoCodes = _context.PromoCodes;

        if (!trackChanges)
            promoCodes = promoCodes.AsNoTracking();

        if (promoCodeParameters.StoreId is not null)
            promoCodes = promoCodes.Where(x => x.StoreId == promoCodeParameters.StoreId);

        if (promoCodeParameters.DiscountType is not null)
            promoCodes = promoCodes.Where(x => x.DiscountType == promoCodeParameters.DiscountType);

        if (promoCodeParameters.IsActive is not null)
            promoCodes = promoCodes.Where(x => x.IsActive == promoCodeParameters.IsActive);

        if (promoCodeParameters.NumberOfTimesUsed is not null)
            promoCodes = promoCodes.Where(x => x.NumberOfTimesUsed == promoCodeParameters.NumberOfTimesUsed);

        if (promoCodeParameters.StartDate is not null)
            promoCodes = promoCodes.Where(x => promoCodeParameters.StartDate <= x.StartDate);

        if (promoCodeParameters.EndDate is not null)
            promoCodes = promoCodes.Where(x => x.EndDate <= promoCodeParameters.EndDate);

        if (promoCodeParameters.Descending)
        {
            if (promoCodeParameters.OrderByDiscountValue)
                promoCodes = promoCodes.OrderByDescending(x => x.DiscountValue);
            else if (promoCodeParameters.OrderByStartDate)
                promoCodes = promoCodes.OrderByDescending(x => x.StartDate);

            else if (promoCodeParameters.OrderByEndDate)
                promoCodes = promoCodes.OrderByDescending(x => x.EndDate);

            else if (promoCodeParameters.OrderByUsageLimit)
                promoCodes = promoCodes.OrderByDescending(x => x.UsageLimit);

            else if (promoCodeParameters.OrderByNumberOfTimesUsed)
                promoCodes = promoCodes.OrderByDescending(x => x.NumberOfTimesUsed);
        }
        else
        {
            if (promoCodeParameters.OrderByDiscountValue)
                promoCodes = promoCodes.OrderBy(x => x.DiscountValue);

            else if (promoCodeParameters.OrderByStartDate)
                promoCodes = promoCodes.OrderBy(x => x.StartDate);

            else if (promoCodeParameters.OrderByEndDate)
                promoCodes = promoCodes.OrderBy(x => x.EndDate);

            else if (promoCodeParameters.OrderByUsageLimit)
                promoCodes = promoCodes.OrderBy(x => x.UsageLimit);

            else if (promoCodeParameters.OrderByNumberOfTimesUsed)
                promoCodes = promoCodes.OrderBy(x => x.NumberOfTimesUsed);
        }

        int count = await promoCodes.CountAsync();

        List<PromoCodeDto> promoCodeDtos = await promoCodes.Select(x => new PromoCodeDto
        {
            Id = x.Id,
            Code = x.Code,
            DiscountType = x.DiscountType,
            DiscountValue = x.DiscountValue,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            UsageLimit = x.UsageLimit,
            NumberOfTimesUsed = x.NumberOfTimesUsed,
            IsActive = x.IsActive
        })
        .Skip((promoCodeParameters.PageNumber - 1) * promoCodeParameters.PageSize)
        .Take(promoCodeParameters.PageSize)
        .ToListAsync();

        return new PagedList<PromoCodeDto>(promoCodeDtos, count, promoCodeParameters.PageNumber, promoCodeParameters.PageSize);
    }

    public async Task<int> GetUsageCountByCodeAsync(string code)
        => await _context.PromoCodes.Where(x => x.Code == code).Select(x => x.NumberOfTimesUsed).FirstOrDefaultAsync();

    public async Task<int> GetUsageCountByIdAsync(Guid promoCodeId)
        => await _context.PromoCodes.Where(x => x.Id == promoCodeId).Select(x => x.NumberOfTimesUsed).FirstOrDefaultAsync();

    public async Task<bool> CheckPromoCodeUniqueByCodeAsync(string code)
        => !await _context.PromoCodes.AnyAsync(x => x.Code == code);

    public async Task<PromoCode?> PromoCodeExistsByCodeAsync(string code)
        => await _context.PromoCodes.FirstOrDefaultAsync(x => x.Code == code);

    public void CreatePromoCode(PromoCode promoCode)
        => _context.PromoCodes.Add(promoCode);

    public void UpdatePromoCode(PromoCode promoCode)
        => _context.PromoCodes.Update(promoCode);

    public void DeletePromoCode(PromoCode promoCode)
        => _context.PromoCodes.Remove(promoCode);

    public async Task<int> TotalPromoCodesByStoreIdAsync(Guid storeId)
        => await _context.PromoCodes.CountAsync(x => x.StoreId == storeId);

    public async Task<bool> CheckPromoCodeExistsByIdAsync(Guid promoCodeId)
        => await _context.PromoCodes.AnyAsync(x => x.Id == promoCodeId);

    public async Task<bool> CheckPromoCodeExistsByCodeAsync(string code)
        => await _context.PromoCodes.AnyAsync(x => x.Code == code);

    public async Task<bool> CheckPromoCodeExistsByIdAndOwnedByStoreAsync(Guid promoCodeId, Guid storeId)
        => await _context.PromoCodes.AnyAsync(x => x.Id == promoCodeId && x.StoreId == storeId);
}