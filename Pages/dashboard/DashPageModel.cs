using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Filters;
using Auditor.Models;
using Auditor.Services.Interfaces;

namespace Auditor.Pages.dashboard;

public class DashPageModel : PageModel
{
    public IUserManager UserManager { get; private set; } = null!;
    public IBoardManager BoardManager { get;  private set; } = null!;


    public DashPageModel(IUserManager userManager, IBoardManager boardManager)
    {
        UserManager = userManager;
        BoardManager = boardManager;
    }

    [BindProperty(SupportsGet = true)]
    public int DashboardId { get; set; }

    public Board CurrentBoard { get; set; } = null!;

    public User CurrentUser { get; set; } = null!;

    public async override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        if (DashboardId <= 0)
        {
            context.Result = new BadRequestResult();
            return;
        }

        var board = await BoardManager.GetBoardAsync(DashboardId);

        if(board == null)
        {
            context.Result = new NotFoundResult();
            return;
        }

        CurrentUser = (await UserManager.GetCurrentUserAsync(User))!;
        CurrentBoard = board;

        await next();
    }
}