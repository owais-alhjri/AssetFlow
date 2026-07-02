using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class AssignmentErrors
{
    public static readonly Error AlreadyReturned =
        new("Assignment.AlreadyReturned", "This assignment has already been returned.");

    public static readonly Error ReturnDateBeforeAssignedDate =
        new("Assignment.ReturnDateBeforeAssignedDate", "Return date cannot be before the assigned date.");
}