using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Data;
using Auditor.Models;

namespace Auditor.Pages.Dashboard;

public class FlagModel : PageModel
{
    private readonly AuditorDb _db;

    public FlagModel(AuditorDb db)
    {
        _db = db;
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

        if (!string.IsNullOrEmpty(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        return RedirectToPage("/Dashboard/Index");
    }
}