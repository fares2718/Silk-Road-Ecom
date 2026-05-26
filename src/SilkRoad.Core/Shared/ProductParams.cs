namespace SilkRoad.Core;

public class ProductParams
{
    public int? CategoryID { get; set; } = 0;
    public string? SortBy { get; set; } = null;
    public bool IsDescending { get; set; } = false;

    private int _MaxPageSize = 6;

    private int _PageSize = 3;
    public int PageSize
    {
        get { return _PageSize; }
        set
        { 
            _PageSize = value > _MaxPageSize ? _MaxPageSize : value ;
        }
    }
    public int PageNumber { get; set; } = 1;
    public string? Search { get; set; } = null; 
}
