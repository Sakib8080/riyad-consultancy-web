using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Web.Areas.Admin.Models;

namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProfileController : Controller
{
    private readonly IIdentityService _identity;

    public ProfileController(IIdentityService identity) => _identity = identity;

    [HttpGet]
    public IActionResult ChangePassword() => View(new ChangePasswordViewModel());

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var result = await _identity.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        TempData["Success"] = "Password changed.";
        return RedirectToAction(nameof(ChangePassword));
    }
}
