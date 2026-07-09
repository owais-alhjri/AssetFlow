using AssetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data;

public class AssetFlowDbContext(DbContextOptions<AssetFlowDbContext> options) : DbContext(options)
{
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AssetStatus> AssetStatuses => Set<AssetStatus>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetFlowDbContext).Assembly);
    }

}