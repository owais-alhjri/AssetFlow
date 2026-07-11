using AssetFlow.Application.Auth.Commands.LoginUser;
using AssetFlow.Application.Auth.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AssetFlow.API.Controllers;

[Route("api/[controller]")]
public class AuthController(ISender sender) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Problem(result.Error!);
    }
}