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

    public static readonly Error NotPending =
        new("User.NotPending", "This registration has already been reviewed.");

    public static readonly Error AccountPending =
        new("User.AccountPending", "Your account is awaiting admin approval.");

    public static readonly Error AccountRejected =
        new("User.AccountRejected", "Your registration was not approved.");

    public static Error NotFound(Guid id) =>
        new("User.NotFound", $"No user was found with ID '{id}'.");
}