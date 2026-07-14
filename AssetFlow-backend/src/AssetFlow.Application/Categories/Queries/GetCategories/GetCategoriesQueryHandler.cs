using AssetFlow.Application.Common.DTOs;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<CategoryDto>>>
{
    public async Task<Result<IReadOnlyList<CategoryDto>>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken
    )
    {   
        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        var items = categories.Select(CategoryDto.FromEntity).ToList();
        return items;
    }
}