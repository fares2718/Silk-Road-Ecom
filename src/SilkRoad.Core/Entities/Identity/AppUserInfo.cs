namespace SilkRoad.Core;

public partial class AppUserInfo
{
    public int AppUserInfoID { get; set; }
    public int CityID { get; set; }
    public City City { get; set; } = null!;
    public string? Street { get; set; }
    public string ZipCode { get; set; } = null!;
    public string AppUserID { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
}
