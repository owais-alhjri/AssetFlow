using FluentValidation;

namespace AssetFlow.Application.Assets.Commands.CreateAsset;

public class CreateAssetCommandValidator : AbstractValidator<CreateAssetCommand>
{
    public CreateAssetCommandValidator()
    {
        RuleFor(x => x.Tag).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SerialNumber).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Condition).IsInEnum();
        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Currency)
            .Length(3).WithMessage("Currency must be a 3-letter code.")
            .When(x => !string.IsNullOrWhiteSpace(x.Currency));
    }
}