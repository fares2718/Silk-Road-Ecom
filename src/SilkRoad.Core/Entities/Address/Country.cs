namespace SilkRoad.Core;

public partial class Country
{
    public int CountryID { get; set; }
    public string CountryName { get; set; } = null!;
    public string CountryCode { get; set; } = null!;

    public ICollection<State> States { get; set; } = new List<State>();
}
