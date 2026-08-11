using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Dtos;

namespace Auditor.Pages.Login;

public class ResetModel : PageModel
{
    [BindProperty]
    public PasswordReset Input { get; set; } = new PasswordReset();
    public async Task OnGetAsync()
    {
    }
}