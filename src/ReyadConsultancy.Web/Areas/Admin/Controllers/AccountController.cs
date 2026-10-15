using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Web.Areas.Admin.Models;

namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IIdentityService _identity;

    public AccountController(IIdentityService identity) => _identity = identity;

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Dashboard");
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(model);

        var result = await _identity.LoginAsync(model.Email, model.Password, model.RememberMe);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl)
            : RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Dashboard");
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _identity.RegisterAsync(model.FullName, model.Email, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        TempData["Success"] = result.PendingApproval
            ? "Registration received. An administrator must approve your account before you can sign in."
            : "Administrator account created. You can sign in now.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _identity.LogoutAsync();
        return Redirect("/");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
