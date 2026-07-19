using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Data;
using Auditor.Models;
using Microsoft.EntityFrameworkCore;

namespace Auditor.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly AuditorDb _db;
        [BindProperty]
        public Transaction IncomingTransaction {get; set;} = null!;

        public IndexModel(AuditorDb db)
        {
            _db = db;
        }

        public List<Transaction> Transactions { get; set; } = null!;
        public async Task OnGetAsync()
        {
            Transactions = await _db.Transactions.OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).Take(10).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Transactions.Add(IncomingTransaction);
            await _db.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
