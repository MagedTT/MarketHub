namespace MarketHub.Application.Contracts.Persistence;

public interface IUserRepository
{
    Task<bool> CheckUserExistsAsync(Guid userId);
}