using FluentValidation;

namespace AssetFlow.Application.Assets.Commands.UpdateAsset;

public class UpdateAssetCommandValidator : AbstractValidator<UpdateAssetCommand>
{
    public UpdateAssetCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Condition).IsInEnum();
    }
}