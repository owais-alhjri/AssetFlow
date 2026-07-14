using AssetFlow.Domain.Common;

namespace AssetFlow.Domain.Errors;

public static class CategoryErrors
{
    public static readonly Error NameEmpty =
        new("Category.NameEmpty", "Category name cannot be empty.");

    public static readonly Error NameTooLong =
        new("Category.NameTooLong", "Category name cannot exceed 200 characters.");
}