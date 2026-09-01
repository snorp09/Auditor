using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Auditor.Pages;

[Authorize]
public class AuthTestModel : PageModel
{
    void OnGet()
    {

    }
}