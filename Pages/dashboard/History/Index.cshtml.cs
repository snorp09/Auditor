using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Data;
using Auditor.Models;
using Microsoft.EntityFrameworkCore;

namespace Auditor.Pages.Dashboard.History;

public class IndexModel : PageModel
{
    private readonly AuditorDb _db;

    public IndexModel(AuditorDb db)
    {
        _db = db;
    }

    public List<Transaction> Transactions { get; set; } = null!;
    public DateTime month { get; set; } = DateTime.Now;
    public async Task OnGetAsync(DateTime? Month = null)
    {
        if(Month.HasValue)
        {
            month = Month.Value;
            Transactions = await _db.Transactions.Where(t => t.Date.Month == Month.Value.Month && t.Date.Year == Month.Value.Year).OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).ToListAsync();
            return;
        }
        month = DateTime.Now;
        Transactions = await _db.Transactions.Where(t => t.Date.Month == month.Month && t.Date.Year == month.Year).OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).ToListAsync();
        return;
    }
}