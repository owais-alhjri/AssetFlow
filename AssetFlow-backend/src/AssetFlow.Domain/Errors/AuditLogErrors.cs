using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class AuditLogErrors
{
    public static readonly Error ActionEmpty =
        new("AuditLog.ActionEmpty", "Audit log action cannot be empty.");

    public static readonly Error EntityNameEmpty =
        new("AuditLog.EntityNameEmpty", "Audit log entity name cannot be empty.");
}