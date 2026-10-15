using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class FaqsController : CrudController<Faq>
{
    public FaqsController(ICrudService<Faq> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "Faq";
    protected override string ListTitle => "FAQs";

    protected override IReadOnlyList<CrudColumn<Faq>> Columns => new[]
    {
        CrudColumn<Faq>.Text("Question", e => e.Question),
        CrudColumn<Faq>.Text("Order", e => e.SortOrder),
        CrudColumn<Faq>.Bool("Active", e => e.IsActive)
    };

}
