using FluentValidation;

namespace AssetFlow.Application.Assets.Commands.DeleteAsset;

public class DeleteAssetCommandValidator :AbstractValidator<DeleteAssetCommand>
{
    public DeleteAssetCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}