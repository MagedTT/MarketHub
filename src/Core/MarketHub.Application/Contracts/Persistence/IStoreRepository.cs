namespace MarketHub.Application.Contracts.Persistence;

public interface IStoreRepository
{
    Task<bool> CheckStoreExistsAsync(Guid id, bool trackChanges);
}