using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Application.Options;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;


public class EmailService : IEmailService
{
    private readonly SmtpOptions _options;

    public EmailService(IOptions<SmtpOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string body,
        string? attachmentName = null,
        byte[]? attachmentBytes = null)
    {
        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            Credentials = new NetworkCredential(_options.User, _options.Password),
            EnableSsl = _options.EnableSsl
        };

        using var message = new MailMessage
        {
            From = new MailAddress(_options.User),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);

        if (attachmentBytes != null && attachmentName != null)
        {
            var stream = new MemoryStream(attachmentBytes);
            message.Attachments.Add(new Attachment(stream, attachmentName));
        }

        await client.SendMailAsync(message);
    }
}
