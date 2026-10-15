using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Website;
using ReyadConsultancy.Web.Models;

namespace ReyadConsultancy.Web.Controllers;

public class BlogController : Controller
{
    private const int PageSize = 5;
    private readonly IWebsiteContentService _content;
    private readonly IWebsiteSubmissionService _submissions;

    public BlogController(IWebsiteContentService content, IWebsiteSubmissionService submissions)
    {
        _content = content;
        _submissions = submissions;
    }

    [HttpGet("blog")]
    public async Task<IActionResult> Index(int page = 1, string? category = null, string? q = null, string? tag = null)
        => View(await _content.GetBlogListAsync(page, PageSize, category, q, tag));

    [HttpGet("blog/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var model = await _content.GetBlogPostAsync(slug);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost("blog/{slug}/comment")]
    public async Task<IActionResult> Comment(string slug, CommentForm form)
    {
        if (!string.IsNullOrEmpty(form.Website)) return RedirectToAction(nameof(Details), new { slug });
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please fill in your name, a valid email and a message.";
            return Redirect(Url.Action(nameof(Details), new { slug }) + "#respond");
        }

        var ok = await _submissions.AddCommentAsync(slug, form.Name.Trim(), form.Email.Trim(), form.Phone?.Trim(), form.Message.Trim());
        if (!ok) return NotFound();
        TempData["Success"] = "Thank you! Your comment will appear after it has been approved.";
        return Redirect(Url.Action(nameof(Details), new { slug }) + "#respond");
    }
}
