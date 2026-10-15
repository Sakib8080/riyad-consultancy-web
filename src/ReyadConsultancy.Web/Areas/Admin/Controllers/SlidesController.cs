using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SlidesController : CrudController<Slide>
{
    public SlidesController(ICrudService<Slide> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "Slide";
    protected override string ListTitle => "Home Slider";

    protected override string[] ImageProperties => new[] { "ImageUrl" };

    protected override IReadOnlyList<CrudColumn<Slide>> Columns => new[]
    {
        CrudColumn<Slide>.Image("Image", e => e.ImageUrl),
        CrudColumn<Slide>.Text("Title", e => e.Title),
        CrudColumn<Slide>.Text("Order", e => e.SortOrder),
        CrudColumn<Slide>.Bool("Active", e => e.IsActive)
    };

}
