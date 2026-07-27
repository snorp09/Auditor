using Auditor.Services.Interfaces;
using Auditor.Models;
using Auditor.Configs;
using Microsoft.Extensions.Options;
namespace Auditor.Services;

public class FlagNotificationProvider : IFlagNotificationProvider
{
    private readonly IEmailService _emailService;
    private readonly EmailConfig _emailConfig;

    public FlagNotificationProvider(IEmailService emailService, IOptions<EmailConfig> emailConfig)
    {
        _emailService = emailService;
        _emailConfig = emailConfig.Value;
    }

    public async Task NotifyFlaggedTransactionAsync(Transaction transaction)
    {
        string subject = $"Flagged Transaction: {transaction.Name}";
        string body = $"A transaction has been flagged:\n\n" +
                      $"Name: {transaction.Name}\n" +
                      $"Amount: {transaction.Amount}\n" +
                      $"Date: {transaction.Date}\n" +
                      $"Type: {transaction.Type}\n";

        await _emailService.SendEmailAsync(_emailConfig.ToEmail, subject, body);
    }
}