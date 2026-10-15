using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class TeamMembersController : CrudController<TeamMember>
{
    public TeamMembersController(ICrudService<TeamMember> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "TeamMember";
    protected override string ListTitle => "Team Members";

    protected override string[] ImageProperties => new[] { "PhotoUrl" };

    protected override IReadOnlyList<CrudColumn<TeamMember>> Columns => new[]
    {
        CrudColumn<TeamMember>.Image("Photo", e => e.PhotoUrl),
        CrudColumn<TeamMember>.Text("Name", e => e.Name),
        CrudColumn<TeamMember>.Text("Designation", e => e.Designation),
        CrudColumn<TeamMember>.Text("Order", e => e.SortOrder),
        CrudColumn<TeamMember>.Bool("Active", e => e.IsActive)
    };

}
