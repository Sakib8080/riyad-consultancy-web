using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;
using ReyadConsultancy.Application.Common;

namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ServicesController : CrudController<Service>
{
    public ServicesController(ICrudService<Service> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "Service";
    protected override string ListTitle => "Services";

    protected override string[] ImageProperties => new[] { "ImageUrl", "IconUrl", "BannerImageUrl" };

    protected override IReadOnlyList<CrudColumn<Service>> Columns => new[]
    {
        CrudColumn<Service>.Image("Image", e => e.ImageUrl),
        CrudColumn<Service>.Text("Title", e => e.Title),
        CrudColumn<Service>.Text("Slug", e => e.Slug),
        CrudColumn<Service>.Text("Order", e => e.SortOrder),
        CrudColumn<Service>.Bool("Active", e => e.IsActive)
    };

    protected override async Task BeforeSaveAsync(Service model, bool isNew)
    {
        var all = await Crud.ListAsync();
        var source = string.IsNullOrWhiteSpace(model.Slug) ? model.Title : model.Slug!;
        model.Slug = Slugger.Unique(Slugger.Create(source), all.Where(x => x.Id != model.Id).Select(x => x.Slug));
    }
}
