namespace MarketHub.Application.Contracts.Persistence;

public interface IRepositoryManager
{
    IProductRepository ProductRepository { get; }
    Task<int> SaveAsync();
}