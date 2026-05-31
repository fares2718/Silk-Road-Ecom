using Microsoft.AspNetCore.Identity;

namespace SilkRoad.Core;

public partial class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public AppUserInfo? AppUserInfo { get; set; }
}
