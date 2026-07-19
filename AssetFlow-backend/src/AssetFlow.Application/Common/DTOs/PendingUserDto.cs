using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.DTOs;

public class PendingUserDto
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Status { get; init; }
    public DateTime RequestedAt { get; init; }

    public static PendingUserDto FromEntity(User user) => new()
    {
        Id = user.Id,
        Email = user.Email.Value,
        Status = user.Status.ToString(),
        RequestedAt = user.CreatedAt
    };
}