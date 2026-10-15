using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Website;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Models;

namespace ReyadConsultancy.Web.Controllers;

public class HomeController : Controller
{
    private readonly IWebsiteContentService _content;
    private readonly IWebsiteSubmissionService _submissions;

    public HomeController(IWebsiteContentService content, IWebsiteSubmissionService submissions)
    {
        _content = content;
        _submissions = submissions;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await _content.GetHomeAsync());

    [HttpGet("about")]
    public async Task<IActionResult> About() => View(await _content.GetAboutAsync());

    [HttpGet("contact")]
    public IActionResult Contact() => View(new ContactForm());

    [HttpPost("contact")]
    public async Task<IActionResult> Contact(ContactForm form)
    {
        if (!string.IsNullOrEmpty(form.Website)) return RedirectToAction(nameof(Contact)); // bot
        if (!ModelState.IsValid) return View(form);

        await _submissions.SubmitContactAsync(new ContactMessage
        {
            Name = form.Name.Trim(), Phone = form.Phone.Trim(), Email = form.Email?.Trim(),
            Address = form.Address?.Trim(), Message = form.Message.Trim()
        });
        TempData["Success"] = "Thank you! Your message has been sent. We will contact you soon.";
        return RedirectToAction(nameof(Contact));
    }

    [HttpGet("appointment")]
    public async Task<IActionResult> Appointment()
    {
        ViewBag.WhyChoose = await _content.GetPageAsync("why-choose-us");
        return View(new AppointmentForm());
    }

    [HttpPost("appointment")]
    public async Task<IActionResult> Appointment(AppointmentForm form, string? returnUrl)
    {
        if (!string.IsNullOrEmpty(form.Website)) return RedirectToAction(nameof(Appointment)); // bot
        if (!ModelState.IsValid)
        {
            ViewBag.WhyChoose = await _content.GetPageAsync("why-choose-us");
            return View(form);
        }

        await _submissions.SubmitAppointmentAsync(new Appointment
        {
            FirstName = form.FirstName.Trim(), LastName = form.LastName?.Trim(), Phone = form.Phone.Trim(),
            Email = form.Email?.Trim(), AppointmentDate = form.AppointmentDate, AppointmentTime = form.AppointmentTime,
            CustomerId = form.CustomerId, CaseType = form.CaseType, Gender = form.Gender, Message = form.Message
        });
        TempData["Success"] = "Your appointment request was received. We will confirm it shortly.";
        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl)
            : RedirectToAction(nameof(Appointment));
    }

    [HttpPost("newsletter")]
    public async Task<IActionResult> Subscribe(string? email, string? returnUrl)
    {
        var target = !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? returnUrl : "/";
        if (string.IsNullOrWhiteSpace(email) || !new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email) || email.Length > 150)
        {
            TempData["Error"] = "Please enter a valid email address.";
            return LocalRedirect(target);
        }
        var added = await _submissions.SubscribeAsync(email);
        TempData["Success"] = added ? "Thanks for subscribing!" : "You are already subscribed.";
        return LocalRedirect(target);
    }

    [IgnoreAntiforgeryToken]
    [HttpGet("error/{code:int?}")]
    [HttpPost("error/{code:int?}")]
    public IActionResult Error(int? code)
    {
        Response.StatusCode = code ?? 500;
        return code == 404 ? View("NotFound") : View("Error");
    }
}
