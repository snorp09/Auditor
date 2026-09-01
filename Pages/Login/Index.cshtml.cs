using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Auditor.Dtos;
using Auditor.Services.Interfaces;
using Auditor.Models;

namespace Auditor.Pages.Login;

public class IndexModel : PageModel
{

    private readonly IUserManager _UserManager;
    private readonly IBoardManager _BoardManager;


    [BindProperty]
    public UserLogin Input { get; set; } = new UserLogin();

    public IndexModel(IUserManager userManager, IBoardManager boardManager)
    {
        _UserManager = userManager;
        _BoardManager = boardManager;
    }

    public async Task OnGetAsync()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userResults = await _UserManager.AuthenticateUser(Input.Email, Input.Password);

        if (!userResults.isSuccess)
        {
            ModelState.AddModelError("Email", "Invalid Credentials");
            return Page();
        }

        User user = userResults.user!;

        List<Claim> claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Id.ToString()),
            new(ClaimTypes.Authentication, "Password")
        };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity)
        );

        var board = await _BoardManager.GetUserFirstBoardAsync(user.Id);

        board ??= await _BoardManager.CreateBoardAsync(user);

        return RedirectToPage("/dashboard/Index", new { DashboardId = board.Id });
    }

    public async Task<IActionResult> OnPostForgotPasswordAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _UserManager.GeneratePasswordResetToken(Input.Email);

        return RedirectToPage("/Login/forgotpasswordsent");
    }
}