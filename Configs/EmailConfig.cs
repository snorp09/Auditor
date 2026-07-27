namespace Auditor.Configs;

public class EmailConfig
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;

    // For phase 1, we will send all notifications to a single email address.
    // Once multi-user support is implemented, this can be changed to a list of email addresses.
    public string ToEmail { get; set; } = string.Empty;
}