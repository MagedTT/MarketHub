using MarketHub.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MarketHub.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MarketHubDbContext _context;

    public UserRepository(MarketHubDbContext context)
        => _context = context;

    public async Task<bool> CheckUserExistsAsync(Guid userId)
        => await _context.Users.AnyAsync(x => x.Id == userId);
}