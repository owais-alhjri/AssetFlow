using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;

namespace AssetFlow.Domain.Entities;

public class MaintenanceType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected MaintenanceType() { }

    private MaintenanceType(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsActive = true;
    }

    public static Result<MaintenanceType> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return MaintenanceTypeErrors.NameEmpty;

        if (name.Length > 50)
            return MaintenanceTypeErrors.NameTooLong;

        return new MaintenanceType(name.Trim());
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return MaintenanceTypeErrors.AlreadyInactive;

        IsActive = false;
        return Result.Success();
    }
}