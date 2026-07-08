using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.DTOs;

public class AuthResponseDto
{
    public string Token { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;

    public static AuthResponseDto FromEntity(User user, string token, string roleName) => new()
    {
        Token = token,
        UserId = user.Id,
        Email = user.Email.Value,
        Role = roleName
    };
}