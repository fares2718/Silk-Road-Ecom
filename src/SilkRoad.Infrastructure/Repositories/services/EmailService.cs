using Microsoft.Extensions.Configuration;
using MimeKit;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

internal class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(EmailDTO emailDTO)
    {
        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("Silk Road", _config["EmailSettings:From"] ?? ""));
        message.Subject = emailDTO.Subject;
        message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
        message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = emailDTO.Body
        };

        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            try
            {
                await smtp.ConnectAsync(
                    _config["EmailSettings:Smtp"] ?? "",
                   int.Parse(_config["EmailSettings:Port"] ?? "1"), true);
                await smtp.AuthenticateAsync(_config["EmailSettings:Username"] ?? "",
                    _config["EmailSettings:Password"] ?? "");

                await smtp.SendAsync(message);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                smtp.Disconnect(true);
                smtp.Dispose();
            }
        }
    }
}
