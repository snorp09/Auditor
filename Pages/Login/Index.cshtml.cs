using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using Auditor.Dtos;

namespace Auditor.Pages.Login;

public class IndexModel : PageModel
{
    [BindProperty]
    public UserLogin Input { get; set; } = new UserLogin();

    public async Task OnGetAsync()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Debug.WriteLine(Input);

        return RedirectToPage();
    }
}