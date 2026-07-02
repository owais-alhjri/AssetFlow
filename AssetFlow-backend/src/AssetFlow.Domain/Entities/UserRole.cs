using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;

namespace AssetFlow.Domain.Entities;

public class UserRole
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected UserRole() { }

    private UserRole(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsActive = true;
    }

    public static Result<UserRole> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return UserRoleErrors.NameEmpty;

        if (name.Length > 50)
            return UserRoleErrors.NameTooLong;

        return new UserRole(name.Trim());
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return UserRoleErrors.AlreadyInactive;

        IsActive = false;
        return Result.Success();
    }
}