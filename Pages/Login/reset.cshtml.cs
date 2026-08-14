using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Dtos;
using Auditor.Services.Interfaces;

namespace Auditor.Pages.Login;

public class ResetModel : PageModel
{
    private readonly IUserManager _userManager;
    public ResetModel(IUserManager userManager)
    {
        _userManager = userManager;
    }
    [BindProperty]
    public PasswordReset Input { get; set; } = new PasswordReset();
    public async Task OnGetAsync()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Handle the password reset logic here using Input.Token and Input.Password
        await _userManager.ResetUserPassword(Input.Token, Input.Password);

        return RedirectToPage("/Login/Index");
    }
}