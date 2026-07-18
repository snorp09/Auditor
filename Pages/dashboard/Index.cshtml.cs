using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Models;

namespace Auditor.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Transaction IncomingTransaction {get; set;}

        public static List<Transaction> TransactionsList =
        [
            new Transaction { Id = 1, Name = "Transaction 1", Amount = 100.00m, Date = DateTime.Now },
            new Transaction { Id = 2, Name = "Transaction 2", Amount = 200.00m, Date = DateTime.Now },
            new Transaction { Id = 3, Name = "Transaction 3", Amount = 300.00m, Date = DateTime.Now }
        ];

        public List<Transaction> Transactions { get; set; } = null!;
        public void OnGet()
        {
            this.Transactions = TransactionsList;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            IncomingTransaction.Id = TransactionsList.Count + 1;

            TransactionsList.Add(IncomingTransaction);
            return RedirectToPage();
        }
    }
}
