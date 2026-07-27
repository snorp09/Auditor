using MailKit.Net.Smtp;
using MimeKit;
using Auditor.Services.Interfaces;
using Auditor.Configs;
using Microsoft.Extensions.Options;

namespace Auditor.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfig _emailConfig;

    public EmailService(IOptions<EmailConfig> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailConfig.FromEmail, _emailConfig.FromEmail));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.SmtpPort, false);
        await client.AuthenticateAsync(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}