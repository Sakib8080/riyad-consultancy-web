using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AppointmentsController : CrudController<Appointment>
{
    public AppointmentsController(ICrudService<Appointment> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "Appointment";
    protected override string ListTitle => "Appointments";

    protected override bool CanCreate => false;

    protected override IReadOnlyList<CrudColumn<Appointment>> Columns => new[]
    {
        CrudColumn<Appointment>.Text("Name", e => e.FirstName),
        CrudColumn<Appointment>.Text("Phone", e => e.Phone),
        CrudColumn<Appointment>.Date("Preferred date", e => e.AppointmentDate),
        CrudColumn<Appointment>.Text("Status", e => e.Status),
        CrudColumn<Appointment>.Date("Requested", e => e.CreatedAt)
    };

}
