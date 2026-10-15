using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Helpers;

namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SettingsController : Controller
{
    private static readonly string[] ImageProps = { nameof(SiteSetting.LogoUrl), nameof(SiteSetting.FaviconUrl), nameof(SiteSetting.FooterImageUrl) };

    private readonly ICrudService<SiteSetting> _settings;
    private readonly IMediaService _media;

    public SettingsController(ICrudService<SiteSetting> settings, IMediaService media)
    {
        _settings = settings;
        _media = media;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var current = (await _settings.ListAsync()).OrderBy(s => s.Id).FirstOrDefault();
        return View(current ?? new SiteSetting());
    }

    [HttpPost]
    public async Task<IActionResult> Index(SiteSetting model)
    {
        if (!ModelState.IsValid) return View(model);

        var existing = model.Id == 0 ? null : await _settings.GetAsync(model.Id);
        var obsolete = new List<string?>();
        foreach (var prop in ImageProps)
        {
            var pi = typeof(SiteSetting).GetProperty(prop)!;
            var file = Request.Form.Files[prop + "File"];
            if (file == null || file.Length == 0) continue;
            try
            {
                var url = await MediaUpload.SaveAsync(_media, file);
                if (existing != null) obsolete.Add(pi.GetValue(existing) as string);
                pi.SetValue(model, url);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(prop, ex.Message);
                return View(model);
            }
        }

        if (existing == null) await _settings.CreateAsync(model);
        else await _settings.UpdateAsync(model);

        foreach (var url in obsolete) await _media.DeleteByUrlAsync(url);
        TempData["Success"] = "Site settings saved.";
        return RedirectToAction(nameof(Index));
    }
}
