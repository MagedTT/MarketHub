namespace MarketHub.Application.Contracts.Persistence;

public interface IBrandRepository
{
    Task<bool> CheckBrandExistsAsync(Guid id, bool trackChanges);
}