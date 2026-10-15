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
public class BlogCategoriesController : CrudController<BlogCategory>
{
    public BlogCategoriesController(ICrudService<BlogCategory> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "BlogCategory";
    protected override string ListTitle => "Blog Categories";

    protected override IReadOnlyList<CrudColumn<BlogCategory>> Columns => new[]
    {
        CrudColumn<BlogCategory>.Text("Name", e => e.Name),
        CrudColumn<BlogCategory>.Text("Slug", e => e.Slug)
    };

    protected override async Task BeforeSaveAsync(BlogCategory model, bool isNew)
    {
        var all = await Crud.ListAsync();
        var source = string.IsNullOrWhiteSpace(model.Slug) ? model.Name : model.Slug!;
        model.Slug = Slugger.Unique(Slugger.Create(source), all.Where(x => x.Id != model.Id).Select(x => x.Slug));
    }
}
