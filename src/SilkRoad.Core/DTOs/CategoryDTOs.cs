namespace SilkRoad.Core;

public record class AddCategoryDTO
(    string CategoryName,
    string? CategoryDescription
);

public record CategoryDTO
(
    int CategoryID,
    string CategoryName,
    string? CategoryDescription
);