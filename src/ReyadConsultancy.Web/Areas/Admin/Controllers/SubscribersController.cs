using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SubscribersController : CrudController<NewsletterSubscriber>
{
    public SubscribersController(ICrudService<NewsletterSubscriber> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "NewsletterSubscriber";
    protected override string ListTitle => "Newsletter Subscribers";

    protected override IReadOnlyList<CrudColumn<NewsletterSubscriber>> Columns => new[]
    {
        CrudColumn<NewsletterSubscriber>.Text("Email", e => e.Email),
        CrudColumn<NewsletterSubscriber>.Date("Subscribed", e => e.CreatedAt),
        CrudColumn<NewsletterSubscriber>.Bool("Active", e => e.IsActive)
    };

}
