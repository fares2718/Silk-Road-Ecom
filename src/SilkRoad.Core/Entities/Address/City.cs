namespace SilkRoad.Core;

public class City
{
    public int CityID { get; set; }
    public string CityName { get; set; } = null!;
    public int StateID { get; set; }
    public State State { get; set; } = null!;
    public ICollection<AppUserInfo> AppUsersInfos { get; set; } = new List<AppUserInfo>();
}
