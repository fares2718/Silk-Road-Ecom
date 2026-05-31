namespace SilkRoad.Core;

public class EmailDTO
{
    public EmailDTO(string to, string from, string subject, string body)
    {
        To = to;
        From = from;
        Subject = subject;
        Body = body;
    }
    public string To { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
