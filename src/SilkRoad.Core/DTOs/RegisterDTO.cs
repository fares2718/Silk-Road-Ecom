namespace SilkRoad.Core;

public record RegisterDTO
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }
    public string LasName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
