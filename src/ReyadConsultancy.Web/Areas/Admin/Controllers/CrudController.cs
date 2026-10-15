using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Web.Areas.Admin.Helpers;
using ReyadConsultancy.Web.Areas.Admin.Models;

namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

/// <summary>
/// Generic list / create / edit / delete controller used by every content type.
/// Each concrete controller only declares its columns, image properties and (optionally) hooks.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin")]
public abstract class CrudController<T> : Controller where T : BaseEntity, new()
{
    protected readonly ICrudService<T> Crud;
    protected readonly IMediaService Media;

    protected CrudController(ICrudService<T> crud, IMediaService media)
    {
        Crud = crud;
        Media = media;
    }

    protected abstract string EntityTitle { get; }
    protected abstract string ListTitle { get; }
    protected abstract IReadOnlyList<CrudColumn<T>> Columns { get; }

    protected virtual string[] ImageProperties => Array.Empty<string>();
    protected virtual Expression<Func<T, object>>[] Includes => Array.Empty<Expression<Func<T, object>>>();
    protected virtual bool CanCreate => true;

    protected virtual Task LoadLookupsAsync() => Task.CompletedTask;
    protected virtual Task BeforeSaveAsync(T model, bool isNew) => Task.CompletedTask;
    protected virtual Task OnOpenAsync(T model) => Task.CompletedTask;

    [HttpGet]
    public virtual async Task<IActionResult> Index()
    {
        var items = await Crud.ListAsync(Includes);
        var vm = new CrudIndexViewModel
        {
            Title = ListTitle,
            EntityTitle = EntityTitle,
            CanCreate = CanCreate,
            Headers = Columns.Select(c => c.Header).ToList()
        };
        foreach (var item in items)
        {
            var row = new CrudRow { Id = item.Id };
            foreach (var col in Columns) row.Cells.Add(col.Render(item));
            vm.Rows.Add(row);
        }
        return View("~/Areas/Admin/Views/Shared/CrudIndex.cshtml", vm);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Create()
    {
        if (!CanCreate) return NotFound();
        await LoadLookupsAsync();
        return View("Form", new T());
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create(T model)
    {
        if (!CanCreate) return NotFound();
        model.Id = 0;
        if (!ModelState.IsValid) return await ReturnFormAsync(model);

        var obsolete = await ApplyUploadsAsync(model, null);
        if (obsolete == null) return await ReturnFormAsync(model);

        await BeforeSaveAsync(model, true);
        await Crud.CreateAsync(model);
        TempData["Success"] = $"{ListTitle}: item created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var model = await Crud.GetAsync(id);
        if (model == null) return NotFound();
        await OnOpenAsync(model);
        await LoadLookupsAsync();
        return View("Form", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Edit(int id, T model)
    {
        if (id != model.Id) return BadRequest();
        var existing = await Crud.GetAsync(id);
        if (existing == null) return NotFound();
        if (!ModelState.IsValid) return await ReturnFormAsync(model);

        var obsolete = await ApplyUploadsAsync(model, existing);
        if (obsolete == null) return await ReturnFormAsync(model);

        await BeforeSaveAsync(model, false);
        await Crud.UpdateAsync(model);
        foreach (var url in obsolete) await Media.DeleteByUrlAsync(url);   // remove replaced images

        TempData["Success"] = $"{ListTitle}: item updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var existing = await Crud.GetAsync(id);
        if (existing != null)
        {
            await Crud.DeleteAsync(id);
            foreach (var prop in ImageProperties)
                await Media.DeleteByUrlAsync(typeof(T).GetProperty(prop)?.GetValue(existing) as string);
        }
        TempData["Success"] = $"{ListTitle}: item deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> ReturnFormAsync(T model)
    {
        await LoadLookupsAsync();
        return View("Form", model);
    }

    /// <summary>Saves posted image files (input name = PropertyName + "File") and returns the URLs that were replaced.</summary>
    private async Task<List<string?>?> ApplyUploadsAsync(T model, T? existing)
    {
        var obsolete = new List<string?>();
        foreach (var prop in ImageProperties)
        {
            var pi = typeof(T).GetProperty(prop)!;
            var file = Request.Form.Files[prop + "File"];
            if (file == null || file.Length == 0) continue;
            try
            {
                var url = await MediaUpload.SaveAsync(Media, file);
                if (existing != null) obsolete.Add(pi.GetValue(existing) as string);
                pi.SetValue(model, url);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(prop, ex.Message);
                return null;
            }
        }
        return obsolete;
    }
}
