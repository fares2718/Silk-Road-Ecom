namespace SilkRoad.Core;
public record LoginDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public record RegisterDTO : LoginDTO
{
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LasName { get; set; } = null!;

}

public record ResetPasswordDTO : LoginDTO
{
    public string Token { get; set; } = null!;
}

public record ActiveAccountDTO
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}
