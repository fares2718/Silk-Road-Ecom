namespace SilkRoad.Core;

public record CountryDTO
{
    public int CountryID { get; set; }
    public string CountryName { get; set; } = null!;
}

public record StateDTO
{
    public int StateID { get; set; }
    public string StateName { get; set; } = null!;
}

public record CityDTO
{
    public int CityID { get; set; }
    public string CityName { get; set; } = null!;
}
