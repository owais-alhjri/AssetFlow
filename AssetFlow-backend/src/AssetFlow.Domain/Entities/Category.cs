using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;

namespace AssetFlow.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected Category(){}

    private Category(string name, string? description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Result<Category> Create(string name, string? description)
    {
        ValidateName(name);
        return new Category(name.Trim(), description?.Trim());
    }

    public  Result Update(string name, string? description)
    {
        ValidateName(name);
        Name = name.Trim();
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return CategoryErrors.NameEmpty;
        if (name.Length > 200)
            return CategoryErrors.NameTooLong;
        return Result.Success();
    }
}