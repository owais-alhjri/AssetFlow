using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class UserRoleErrors
{
    public static readonly Error NameEmpty =
        new("UserRole.NameEmpty", "User role name cannot be empty.");
    public static readonly Error NameTooLong =
        new("UserRole.NameTooLong", "User role name cannot exceed 50 characters.");
    public static readonly Error AlreadyInactive =
        new("UserRole.AlreadyInactive", "User role is already inactive.");
}

