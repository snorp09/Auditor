using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Auditor.Services.Interfaces;

namespace Auditor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserManager _userManager;

    public AuthController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("signout")]
    public async Task<IActionResult> SignUserOut()
    {
        // Clear the authentication cookie
        await HttpContext.SignOutAsync();

        return Redirect("/Login");
    }

    [HttpPost("passwordreset")]
    public async Task<IActionResult> SendResetEmail(string email)
    {
        await _userManager.GeneratePasswordResetToken(email);
        return RedirectToPage("/Login/forgottenpasswordsent");
    }
}