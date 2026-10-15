using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReyadConsultancy.Application.Common;

namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BlogPostsController : CrudController<BlogPost>
{
    public BlogPostsController(ICrudService<BlogPost> crud, IMediaService media, ICrudService<BlogCategory> categories) : base(crud, media)
    {
        _categories = categories;
    }

    protected override string EntityTitle => "BlogPost";
    protected override string ListTitle => "Blog Posts";

    protected override string[] ImageProperties => new[] { "ImageUrl" };

    protected override Expression<Func<BlogPost, object>>[] Includes => new Expression<Func<BlogPost, object>>[] { x => x.Category };

    protected override IReadOnlyList<CrudColumn<BlogPost>> Columns => new[]
    {
        CrudColumn<BlogPost>.Image("Image", e => e.ImageUrl),
        CrudColumn<BlogPost>.Text("Title", e => e.Title),
        CrudColumn<BlogPost>.Text("Category", e => e.Category?.Name),
        CrudColumn<BlogPost>.Date("Published", e => e.PublishedAt),
        CrudColumn<BlogPost>.Bool("Published", e => e.IsPublished)
    };

    private readonly ICrudService<BlogCategory> _categories;

    protected override async Task LoadLookupsAsync()
    {
        var cats = await _categories.ListAsync();
        ViewBag.Categories = cats.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
    }

    protected override async Task BeforeSaveAsync(BlogPost model, bool isNew)
    {
        var all = await Crud.ListAsync();
        var source = string.IsNullOrWhiteSpace(model.Slug) ? model.Title : model.Slug!;
        model.Slug = Slugger.Unique(Slugger.Create(source), all.Where(x => x.Id != model.Id).Select(x => x.Slug));
    }
}
