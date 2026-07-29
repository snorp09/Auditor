using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Auditor.Pages.Login;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}