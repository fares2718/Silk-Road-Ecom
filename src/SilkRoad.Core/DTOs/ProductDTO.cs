namespace SilkRoad.Core;

public record AddProductDTO
(
    string ProductName,
    string? Description,
    int CategoryID,
    decimal Price,
    IReadOnlyList<string> ImageURLs
);

public record ProductDTO
(
    int ProductID,
    string ProductName,
    string? Description,
    string CategoryName,
    decimal Price,
    IReadOnlyList<string> ImageURLs
);

public record UpdateProductDTO
(
    int ProductID,
    string ProductName,
    string? Description,
    decimal Price
);