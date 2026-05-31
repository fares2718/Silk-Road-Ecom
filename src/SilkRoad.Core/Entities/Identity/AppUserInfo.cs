namespace SilkRoad.Core;

public partial class AppUserInfo
{
    public int AppUserInfoID { get; set; }
    public int CityID { get; set; }
    public City City { get; set; } = null!;
    public string? Street { get; set; }
    public string ZipCode { get; set; } = null!;
    public Guid AppUserID { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
