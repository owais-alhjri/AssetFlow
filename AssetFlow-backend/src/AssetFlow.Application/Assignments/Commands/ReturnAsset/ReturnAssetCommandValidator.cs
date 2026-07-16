using FluentValidation;

namespace AssetFlow.Application.Assignments.Commands.ReturnAsset;

public class ReturnAssetCommandValidator : AbstractValidator<ReturnAssetCommand>
{
    public ReturnAssetCommandValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();
        RuleFor(x => x.ConditionAtReturn).IsInEnum();
        RuleFor(x => x.ReturnedDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(x => x.ReturnNotes).MaximumLength(1000);
    }
}