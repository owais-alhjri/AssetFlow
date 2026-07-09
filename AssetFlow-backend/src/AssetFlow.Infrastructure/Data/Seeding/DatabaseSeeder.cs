using System.Text.Json;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Entities;
using AssetFlow.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AssetFlow.Infrastructure.Data.Seeding;

public class DatabaseSeeder(
    AssetFlowDbContext db,
    IPasswordHasher passwordHasher
    )
{
    private static readonly string SeedPath =
        Path.Combine(AppContext.BaseDirectory, "Data", "Seeding", "SeedData");

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedUserRolesAsync(cancellationToken);
        await SeedAssetStatusesAsync(cancellationToken);
        await SeedCategoriesAsync(cancellationToken);
        await SeedUsersAsync(cancellationToken);
        // Employees, Assets, Assignments follow the same pattern later.
    }

    private async Task SeedUserRolesAsync(CancellationToken ct)
    {
        if(await db.UserRoles.AnyAsync(ct)) return;

        var names = await ReadJsonAsync<List<LookupSeed>>("userroles.json");
        foreach (var n in names)
        {
            var result = UserRole.Create(n.Name);
            if (result.IsSuccess)
                db.UserRoles.Add(result.Value!);
        }

        await db.SaveChangesAsync(ct);
    }
    private async Task SeedAssetStatusesAsync(CancellationToken ct)
    {
        if (await db.AssetStatuses.AnyAsync(ct)) return;

        var names = await ReadJsonAsync<List<LookupSeed>>("assetstatuses.json");
        foreach (var n in names)
        {
            var result = AssetStatus.Create(n.Name);
            if (result.IsSuccess)
                db.AssetStatuses.Add(result.Value!);
        }
        await db.SaveChangesAsync(ct);
    }

    private async Task SeedCategoriesAsync(CancellationToken ct)
    {
        if (await db.Categories.AnyAsync(ct)) return;

        var cats = await ReadJsonAsync<List<CategorySeed>>("categories.json");
        foreach (var c in cats)
        {
            var result = Category.Create(c.Name, c.Description);
            if (result.IsSuccess)
                db.Categories.Add(result.Value!);
        }
        await db.SaveChangesAsync(ct);
    }

    private async Task SeedUsersAsync(CancellationToken ct)
    {
        if (await db.Users.AnyAsync(ct)) return;

        var users = await ReadJsonAsync<List<UserSeed>>("users.json");
        foreach (var u in users)
        {
            // Resolve the role id by name — same lookup your handlers do.
            var roleId = await db.UserRoles
                .Where(r => r.Name == u.Role)
                .Select(r => (Guid?)r.Id)
                .FirstOrDefaultAsync(ct);
            if (roleId is null) continue;   // role missing → skip this user

            var emailResult = EmailAddress.Create(u.Email);
            if (!emailResult.IsSuccess) continue;

            // Hash the plain password at seed time, through the real hasher.
            var hash = passwordHasher.Hash(u.Password);
            var hashResult = PasswordHash.Create(hash);
            if (!hashResult.IsSuccess) continue;

            var userResult = User.Create(
                emailResult.Value!, hashResult.Value!, roleId.Value, employeeId: null);
            if (userResult.IsSuccess)
                db.Users.Add(userResult.Value!);
        }
        await db.SaveChangesAsync(ct);
    }


    private static async Task<T> ReadJsonAsync<T>(string fileName)
    {
        var path = Path.Combine(SeedPath, fileName);
        await using var stream = File.OpenRead(path);
        var data = await JsonSerializer.DeserializeAsync<T>(stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return data ?? throw new InvalidOperationException($"Seed file '{fileName}'was empty or invalid.");
    }

    // Plain shapes matching the JSON — NOT the entities.
    private record LookupSeed(string Name);
    private record CategorySeed(string Name, string? Description);
    private record UserSeed(string Email, string Password, string Role);
}