using MailKit.Net.Smtp;
using MimeKit;
using Auditor.Services.Interfaces;
using Auditor.Configs;
using Microsoft.Extensions.Options;

namespace Auditor.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfig _emailConfig;
    private readonly IWebHostEnvironment _env;

    public EmailService(IOptions<EmailConfig> emailConfig, IWebHostEnvironment env)
    {
        _emailConfig = emailConfig.Value;
        _env = env;
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
        try
        {
            await client.AuthenticateAsync(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword);

        }
        catch (System.NotSupportedException)
        {
            if (!_env.IsDevelopment())
            {
                throw;
            }
        }
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}