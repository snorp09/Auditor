using Microsoft.AspNetCore.Mvc;
using Auditor.Services.Interfaces;

namespace Auditor.Components;

public class DashButtonViewComponent : ViewComponent
{

    private readonly IUserManager _userManager;
    private readonly IBoardManager _boardManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashButtonViewComponent(IUserManager userManager, IBoardManager boardManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _boardManager = boardManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if(_httpContextAccessor.HttpContext.User.Identity == null || !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return View(0);
        }

        if(Request.Cookies["DashboardId"] != null)
        {
            var dashboardId = int.Parse(Request.Cookies["DashboardId"]);
            return View(dashboardId);
        }

        var user = await _userManager.GetCurrentUserAsync(_httpContextAccessor.HttpContext.User);
        if (user == null)
        {
            return View(0);
        }

        var board = await _boardManager.GetUserFirstBoardAsync(user.Id);
        return View(board?.Id);
    }
}