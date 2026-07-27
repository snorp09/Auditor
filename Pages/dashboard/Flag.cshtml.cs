using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Data;
using Auditor.Models;
using Auditor.Services.Interfaces;

namespace Auditor.Pages.Dashboard;

public class FlagModel : PageModel
{
    private readonly AuditorDb _db;
    private readonly IFlagNotificationProvider _flagNotificationProvider;

    public FlagModel(AuditorDb db, IFlagNotificationProvider flagNotificationProvider)
    {
        _db = db;
        _flagNotificationProvider = flagNotificationProvider;
    }

    public async Task<IActionResult> OnPostFlagTransactionAsync(int transactionId, string? returnUrl = null)
    {
        var transaction = await _db.Transactions.FindAsync(transactionId);
        if (transaction == null)
        {
            return NotFound();
        }

        transaction.IsFlagged = true;
        await _db.SaveChangesAsync();

        await _flagNotificationProvider.NotifyFlaggedTransactionAsync(transaction);

        if (!string.IsNullOrEmpty(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        return RedirectToPage("/Dashboard/Index");
    }
}