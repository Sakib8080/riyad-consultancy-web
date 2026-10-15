using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class FeaturesController : CrudController<Feature>
{
    public FeaturesController(ICrudService<Feature> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "Feature";
    protected override string ListTitle => "Home Feature Boxes";

    protected override string[] ImageProperties => new[] { "ImageUrl" };

    protected override IReadOnlyList<CrudColumn<Feature>> Columns => new[]
    {
        CrudColumn<Feature>.Image("Image", e => e.ImageUrl),
        CrudColumn<Feature>.Text("Title", e => e.Title),
        CrudColumn<Feature>.Text("Order", e => e.SortOrder),
        CrudColumn<Feature>.Bool("Active", e => e.IsActive)
    };

}
