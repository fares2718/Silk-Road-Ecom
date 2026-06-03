namespace SilkRoad.Core;

public record CompleteAccountDTO
{
    public int AppUserInfoID { get; set; }
    public int CityID { get; set; }
    public string? Street { get; set; }
    public string ZipCode { get; set; } = null!;
    public string AppUserID { get; set; } = null!;
}
