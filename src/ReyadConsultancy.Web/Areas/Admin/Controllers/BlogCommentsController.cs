using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BlogCommentsController : CrudController<BlogComment>
{
    public BlogCommentsController(ICrudService<BlogComment> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "BlogComment";
    protected override string ListTitle => "Blog Comments";

    protected override bool CanCreate => false;

    protected override Expression<Func<BlogComment, object>>[] Includes => new Expression<Func<BlogComment, object>>[] { x => x.BlogPost };

    protected override IReadOnlyList<CrudColumn<BlogComment>> Columns => new[]
    {
        CrudColumn<BlogComment>.Text("Post", e => e.BlogPost?.Title),
        CrudColumn<BlogComment>.Text("Name", e => e.Name),
        CrudColumn<BlogComment>.Text("Message", e => e.Message),
        CrudColumn<BlogComment>.Date("Date", e => e.CreatedAt),
        CrudColumn<BlogComment>.Bool("Approved", e => e.IsApproved)
    };

}
