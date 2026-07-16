using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class AssignmentErrors
{
    public static readonly Error AlreadyReturned =
        new("Assignment.AlreadyReturned", "This assignment has already been returned.");

    public static readonly Error ReturnDateBeforeAssignedDate =
        new("Assignment.ReturnDateBeforeAssignedDate", "Return date cannot be before the assigned date.");
    public static Error NotFound(Guid id) =>
        new("Assignment.NotFound", $"No assignment was found with ID '{id}'.");

    public static readonly Error AssetAlreadyAssigned =
        new("Assignment.AssetAlreadyAssigned",
            "This asset is already assigned and must be returned before it can be reassigned.");
}