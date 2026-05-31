namespace SilkRoad.Core;

public partial class State
{
    public int StateID { get; set; }
    public string StateName { get; set; } = null!;
    public int CountryID { get; set; }
    public Country Country { get; set; } = null!;
    public ICollection<City> Cities { get; set; } = new List<City>();
}
