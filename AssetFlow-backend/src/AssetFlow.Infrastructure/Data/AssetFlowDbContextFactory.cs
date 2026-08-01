// AssetFlow.Infrastructure/Data/AssetFlowDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AssetFlow.Infrastructure.Data;

public class AssetFlowDbContextFactory : IDesignTimeDbContextFactory<AssetFlowDbContext>
{
    public AssetFlowDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AssetFlow.API"))
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "DefaultConnection missing from appsettings.Development.json");

        var options = new DbContextOptionsBuilder<AssetFlowDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new AssetFlowDbContext(options);
    }
}
