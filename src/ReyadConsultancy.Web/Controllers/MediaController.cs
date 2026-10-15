using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;

namespace ReyadConsultancy.Web.Controllers;

/// <summary>Serves images that were uploaded through the admin panel and are stored in MS SQL.</summary>
public class MediaController : Controller
{
    private readonly IMediaService _media;

    public MediaController(IMediaService media) => _media = media;

    [HttpGet("media/{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var file = await _media.GetAsync(id);
        if (file == null) return NotFound();

        // The id is unique and the content never changes, so browsers may cache it for a long time.
        Response.Headers["Cache-Control"] = "public,max-age=31536000,immutable";
        return File(file.Data, file.ContentType);
    }
}
