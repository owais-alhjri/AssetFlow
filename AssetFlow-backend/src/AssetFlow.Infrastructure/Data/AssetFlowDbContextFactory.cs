// AssetFlow.Infrastructure/Data/AssetFlowDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AssetFlow.Infrastructure.Data;

public class AssetFlowDbContextFactory : IDesignTimeDbContextFactory<AssetFlowDbContext>
{
    public AssetFlowDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AssetFlowDbContext>()
            .UseNpgsql("Host=localhost;Database=assetflow;Username=postgres;Password=Owyas90268o")
            .Options;
        return new AssetFlowDbContext(options);
    }
}