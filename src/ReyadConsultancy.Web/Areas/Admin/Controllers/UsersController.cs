using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;

namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly IIdentityService _identity;

    public UsersController(IIdentityService identity) => _identity = identity;

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["CurrentUserId"] = CurrentUserId;
        return View(await _identity.GetUsersAsync());
    }

    [HttpPost]
    public Task<IActionResult> Approve(string id) => SetApproval(id, true);

    [HttpPost]
    public Task<IActionResult> Revoke(string id) => SetApproval(id, false);

    private async Task<IActionResult> SetApproval(string id, bool approved)
    {
        var result = await _identity.SetApprovalAsync(id, approved, CurrentUserId);
        if (result.Succeeded) TempData["Success"] = approved ? "User approved." : "User access revoked.";
        else TempData["Error"] = string.Join(" ", result.Errors);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _identity.DeleteUserAsync(id, CurrentUserId);
        if (result.Succeeded) TempData["Success"] = "User deleted.";
        else TempData["Error"] = string.Join(" ", result.Errors);
        return RedirectToAction(nameof(Index));
    }
}
