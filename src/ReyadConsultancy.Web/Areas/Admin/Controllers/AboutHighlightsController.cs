using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AboutHighlightsController : CrudController<AboutHighlight>
{
    public AboutHighlightsController(ICrudService<AboutHighlight> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "AboutHighlight";
    protected override string ListTitle => "About Page Highlights";

    protected override IReadOnlyList<CrudColumn<AboutHighlight>> Columns => new[]
    {
        CrudColumn<AboutHighlight>.Text("Title", e => e.Title),
        CrudColumn<AboutHighlight>.Text("Icon class", e => e.IconClass),
        CrudColumn<AboutHighlight>.Text("Order", e => e.SortOrder),
        CrudColumn<AboutHighlight>.Bool("Active", e => e.IsActive)
    };

}
