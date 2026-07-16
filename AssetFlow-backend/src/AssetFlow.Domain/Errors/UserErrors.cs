using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class UserErrors
{
    public static readonly Error AlreadyInactive =
        new("User.AlreadyInactive", "User is already inactive.");

    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid email or password.");

    public static readonly Error EmailAlreadyExists =
        new("User.EmailAlreadyExists", "A user with this email already exists.");

    public static readonly Error EmployeeAlreadyLinked =
        new("User.EmployeeAlreadyLinked", "This employee is already linked to a user account.");
}