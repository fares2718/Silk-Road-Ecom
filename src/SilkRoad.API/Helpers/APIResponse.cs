namespace SilkRoad.API;

public class APIResponse
{
    public APIResponse(int statusCode , string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GenerateResponseMessage(statusCode);
    }

    private string GenerateResponseMessage(int statusCode)
    {
        return statusCode switch
        {
            200 => "Success",
            201 => "Created",
            400 => "Invalid Request",
            401 => "Invalid Credentials",
            403 => "Access Denied",
            404 => "Not Found",
            429 => "Too Many Requests",
            500 => "Failure",
            _ => "Unknown Error"
        };
    }

    public int StatusCode  { get; set; }
    public string Message { get; set; } = string.Empty;
}
