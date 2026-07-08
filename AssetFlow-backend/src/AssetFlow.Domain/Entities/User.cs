using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;
using AssetFlow.Domain.ValueObjects;

namespace AssetFlow.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public EmailAddress Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public UserRole Role { get; private set; } = null!;
    public Guid? EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected User() { }

    private User(EmailAddress email, PasswordHash passwordHash, Guid roleId, Guid? employeeId)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        EmployeeId = employeeId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public static Result<User> Create(EmailAddress email, PasswordHash passwordHash,
        Guid roleId, Guid? employeeId)
    {
        return new User(email, passwordHash, roleId, employeeId);
    }

    public Result ChangePassword(PasswordHash newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return UserErrors.AlreadyInactive;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}