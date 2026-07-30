using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Dtos;

namespace Auditor.Pages.Login;

public class SignupModel : PageModel
{
    [BindProperty]
    public UserIncoming Input { get; set; } = new UserIncoming();

    public IActionResult OnGet()
    {
        return Page();
    }
}