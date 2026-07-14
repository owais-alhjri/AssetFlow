using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Infrastructure.Authentication;
using AssetFlow.Infrastructure.Data;
using AssetFlow.Infrastructure.Data.Repositories;
using AssetFlow.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace AssetFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services
    ,IConfiguration configuration)
    {
        services.AddDbContext<AssetFlowDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
        // repositories
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAssetStatusRepository, AssetStatusRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // auth + context
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}