using FluentValidation;

namespace AssetFlow.Application.Auth.Commands.RegisterUser;

public class RegisterUserCommandValidator :AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}