namespace SilkRoad.Core;

public interface IEmailService
{
    Task SendEmailAsync(EmailDTO emailDTO);
}
