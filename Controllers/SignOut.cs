using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Auditor.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public class SignOutController : ControllerBase
{
    [HttpPost("signout")]
    public async Task<IActionResult> SignUserOut()
    {
        // Clear the authentication cookie
        await HttpContext.SignOutAsync();

        return Redirect("/Login");
    }
}