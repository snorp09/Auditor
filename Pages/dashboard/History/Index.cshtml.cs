using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Data;
using Auditor.Models;
using Microsoft.EntityFrameworkCore;
using Auditor.Pages.dashboard;
using Auditor.Services.Interfaces;

namespace Auditor.Pages.Dashboard.History;

public class IndexModel : DashPageModel
{
    private readonly AuditorDb _db;

    public IndexModel(AuditorDb db, IBoardManager boardManager, IUserManager userManager) : base(userManager, boardManager)
    {
        _db = db;
    }

    public List<Transaction> Transactions { get; set; } = null!;

    public decimal TotalIncome => Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
    public decimal TotalExpense => Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
    public decimal TotalBalance => TotalIncome - TotalExpense;

    public DateTime month { get; set; } = DateTime.Now;
    public async Task OnGetAsync(DateTime? Month = null)
    {
        if(Month.HasValue)
        {
            month = Month.Value;
            // Transactions = await _db.Transactions.Where(t => t.Date.Month == Month.Value.Month && t.Date.Year == Month.Value.Year).OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).ToListAsync();
            Transactions = (await BoardManager.GetTransactionsByBoardAsync(CurrentBoard.Id)).Where(t => t.Date.Month == month.Month && t.Date.Year == month.Year).OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).ToList();
            return;
        }
        month = DateTime.Now;
        Transactions = (await BoardManager.GetTransactionsByBoardAsync(CurrentBoard.Id)).Where(t => t.Date.Month == month.Month && t.Date.Year == month.Year).OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).ToList();
        return;
    }
}