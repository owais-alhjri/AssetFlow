using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;

namespace AssetFlow.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }
    public Guid? UserId { get; private set; }
    public string? UserEmail { get; private set; }
    public string Action { get; private set; } = null!;
    public string EntityName { get; private set; } = null!;
    public Guid EntityId { get; private set; }
    public string? Changes { get; private set; }
    public DateTime Timestamp { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected AuditLog() { }

    private AuditLog(Guid? userId, string? userEmail, string action, string entityName,
        Guid entityId, string? changes)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        UserEmail = userEmail;
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        Changes = changes;
        Timestamp = DateTime.UtcNow;
    }

    public static Result<AuditLog> Create(Guid? userId, string? userEmail, string action,
        string entityName, Guid entityId, string? changes)
    {
        if (string.IsNullOrWhiteSpace(action))
            return AuditLogErrors.ActionEmpty;

        if (string.IsNullOrWhiteSpace(entityName))
            return AuditLogErrors.EntityNameEmpty;

        return new AuditLog(userId, userEmail, action.Trim(), entityName.Trim(), entityId, changes);
    }
}

