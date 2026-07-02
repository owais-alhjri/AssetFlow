using AssetFlow.Domain.Common;
using AssetFlow.Domain.Errors;

namespace AssetFlow.Domain.Entities;

public class AssetStatus
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }

    [Obsolete("For EF Core only", error: true)]
    protected AssetStatus() { }

    private AssetStatus(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsActive = true;
    }

    public static Result<AssetStatus> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return AssetStatusErrors.NameEmpty;

        if (name.Length > 50)
            return AssetStatusErrors.NameTooLong;

        return new AssetStatus(name.Trim());
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return AssetStatusErrors.AlreadyInactive;

        IsActive = false;
        return Result.Success();
    }
}
