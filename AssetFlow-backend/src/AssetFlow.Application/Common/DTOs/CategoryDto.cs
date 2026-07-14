using AssetFlow.Domain.Entities;

namespace AssetFlow.Application.Common.DTOs;

public class CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; } = string.Empty;

    public static CategoryDto FromEntity(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
    };

}