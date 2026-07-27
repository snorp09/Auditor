using Auditor.Models;
namespace Auditor.Services.Interfaces;

public interface IFlagNotificationProvider
{
    Task NotifyFlaggedTransactionAsync(Transaction transaction);
}