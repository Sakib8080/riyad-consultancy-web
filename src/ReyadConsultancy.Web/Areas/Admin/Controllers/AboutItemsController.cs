using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AboutItemsController : CrudController<AboutItem>
{
    public AboutItemsController(ICrudService<AboutItem> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "AboutItem";
    protected override string ListTitle => "About (History / Mission / Vision)";

    protected override string[] ImageProperties => new[] { "IconUrl" };

    protected override IReadOnlyList<CrudColumn<AboutItem>> Columns => new[]
    {
        CrudColumn<AboutItem>.Image("Icon", e => e.IconUrl),
        CrudColumn<AboutItem>.Text("Title", e => e.Title),
        CrudColumn<AboutItem>.Text("Order", e => e.SortOrder),
        CrudColumn<AboutItem>.Bool("Active", e => e.IsActive)
    };

}
