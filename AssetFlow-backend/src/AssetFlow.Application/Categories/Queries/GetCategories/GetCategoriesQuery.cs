using AssetFlow.Application.Common.DTOs;
using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery : IRequest<Result<IReadOnlyList<CategoryDto>>>;