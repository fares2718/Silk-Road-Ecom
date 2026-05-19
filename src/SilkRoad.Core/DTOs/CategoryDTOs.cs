namespace SilkRoad.Core;

public record class AddCategoryDTO
(    string CategoryName,
    string? CategoryDescription
);

public record UpdateCategoryDTO
(
    int CategoryID,
    string CategoryName,
    string? CategoryDescription
);