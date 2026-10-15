using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PageContentsController : CrudController<PageContent>
{
    public PageContentsController(ICrudService<PageContent> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "PageContent";
    protected override string ListTitle => "Page Text Blocks";

    protected override string[] ImageProperties => new[] { "ImageUrl" };

    protected override IReadOnlyList<CrudColumn<PageContent>> Columns => new[]
    {
        CrudColumn<PageContent>.Text("Key", e => e.Key),
        CrudColumn<PageContent>.Text("Title", e => e.Title),
        CrudColumn<PageContent>.Image("Image", e => e.ImageUrl)
    };

}
