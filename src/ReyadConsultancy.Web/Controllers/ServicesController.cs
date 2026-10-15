using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Website;

namespace ReyadConsultancy.Web.Controllers;

public class ServicesController : Controller
{
    private readonly IWebsiteContentService _content;

    public ServicesController(IWebsiteContentService content) => _content = content;

    [HttpGet("services")]
    public async Task<IActionResult> Index() => View(await _content.GetServicesAsync());

    [HttpGet("services/list")]
    public async Task<IActionResult> List() => View(await _content.GetServiceListAsync());

    [HttpGet("services/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var model = await _content.GetServiceAsync(slug);
        if (model == null) return NotFound();
        return View(model);
    }
}
