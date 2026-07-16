using FluentValidation;

namespace AssetFlow.Application.Assignments.Commands.AssignAsset;

public class AssignAssetCommandValidator : AbstractValidator<AssignAssetCommand>
{
    public AssignAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.ConditionAtAssign).IsInEnum();
        RuleFor(x => x.AssignedDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}