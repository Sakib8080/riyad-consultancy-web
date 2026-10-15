using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CountersController : CrudController<Counter>
{
    public CountersController(ICrudService<Counter> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "Counter";
    protected override string ListTitle => "Counters";

    protected override string[] ImageProperties => new[] { "IconUrl" };

    protected override IReadOnlyList<CrudColumn<Counter>> Columns => new[]
    {
        CrudColumn<Counter>.Image("Icon", e => e.IconUrl),
        CrudColumn<Counter>.Text("Title", e => e.Title),
        CrudColumn<Counter>.Text("Value", e => e.Value),
        CrudColumn<Counter>.Text("Order", e => e.SortOrder),
        CrudColumn<Counter>.Bool("Active", e => e.IsActive)
    };

}
