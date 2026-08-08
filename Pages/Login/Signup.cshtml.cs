using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Auditor.Services.Interfaces;
using Auditor.Dtos;

namespace Auditor.Pages.Login;

public class SignupModel : PageModel
{
    public SignupModel(IUserManager userManager)
    {
        _userManager = userManager;
    }

    private readonly IUserManager _userManager;

    [BindProperty]
    public UserSignup Input { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _userManager.SignupUser(Input.Name, Input.Email, Input.Password);

        if (!result.isSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Err?.ErrMessage ?? "An error occurred.");
            return Page();
        }

        // Redirect to login page after successful signup
        return RedirectToPage("/Login/Index");
    }
}