using AssetFlow.Application.Common.Interfaces;

namespace AssetFlow.Infrastructure.Data;

public class UnitOfWork(AssetFlowDbContext assetFlowDb) :IUnitOfWork
{
    public  Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return assetFlowDb.SaveChangesAsync(cancellationToken);
    }
}