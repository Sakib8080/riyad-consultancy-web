using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ClientLogosController : CrudController<ClientLogo>
{
    public ClientLogosController(ICrudService<ClientLogo> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "ClientLogo";
    protected override string ListTitle => "Client Logos";

    protected override string[] ImageProperties => new[] { "ImageUrl" };

    protected override IReadOnlyList<CrudColumn<ClientLogo>> Columns => new[]
    {
        CrudColumn<ClientLogo>.Image("Logo", e => e.ImageUrl),
        CrudColumn<ClientLogo>.Text("Name", e => e.Name),
        CrudColumn<ClientLogo>.Text("Order", e => e.SortOrder),
        CrudColumn<ClientLogo>.Bool("Active", e => e.IsActive)
    };

}
