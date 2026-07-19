using FluentValidation;

namespace AssetFlow.Application.Auth.Commands.ApproveUser;

public class ApproveUserCommandValidator : AbstractValidator<ApproveUserCommand>
{
    public ApproveUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.EmployeeNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Department).NotEmpty().MaximumLength(50);
        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
    }
}