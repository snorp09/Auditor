using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Auditor.Data;
using Auditor.Models;
using Auditor.Services.Interfaces;

namespace Auditor.Pages.dashboard
{
    public class IndexModel : DashPageModel
    {
        private readonly AuditorDb _db;
        private readonly IUserManager _userManager;

        [BindProperty]
        public Transaction IncomingTransaction {get; set;} = null!;

        public IndexModel(AuditorDb db, IUserManager userManager, IBoardManager boardManager) : base(userManager, boardManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IEnumerable<Transaction> Transactions { get; set; } = null!;
        public async Task OnGetAsync()
        {
            Transactions = await BoardManager.GetTransactionsByBoardAsync(CurrentBoard.Id);
            CurrentUser = await _userManager.GetCurrentUserAsync(User);
            Response.Cookies.Append("DashboardId", CurrentBoard.Id.ToString() ?? "0");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            IncomingTransaction.BoardId = CurrentBoard.Id;

            _db.Transactions.Add(IncomingTransaction);
            await _db.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
