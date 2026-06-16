using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MarketHub.Persistence;

public class MarketHubDbContextDesignTimeFactory : IDesignTimeDbContextFactory<MarketHubDbContext>
{
    public MarketHubDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<MarketHubDbContext> builder = new DbContextOptionsBuilder<MarketHubDbContext>();

        builder.UseSqlServer(Environment.GetEnvironmentVariable("MarketHub__ConnectionString"));

        return new MarketHubDbContext(builder.Options);
    }
}