using Microsoft.AspNetCore.Http;

namespace SilkRoad.Core;

public record AddProductDTO
(
    string ProductName,
    string? Description,
    int CategoryID,
    decimal NewPrice,
    decimal OldPrice,
    IFormFileCollection ProductImages
);

public record ProductDTO
(
    int ProductID,
    string ProductName,
    string? Description,
    string CategoryName,
    decimal NewPrice,
    decimal OldPrice,
    IReadOnlyList<string> ImageURLs
);

public record UpdateProductDTO
(
    int ProductID,
    string ProductName,
    string? Description,
    decimal Price
);