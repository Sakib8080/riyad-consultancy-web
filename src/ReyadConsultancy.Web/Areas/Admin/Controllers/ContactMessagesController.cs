using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ContactMessagesController : CrudController<ContactMessage>
{
    public ContactMessagesController(ICrudService<ContactMessage> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "ContactMessage";
    protected override string ListTitle => "Contact Messages";

    protected override bool CanCreate => false;

    protected override IReadOnlyList<CrudColumn<ContactMessage>> Columns => new[]
    {
        CrudColumn<ContactMessage>.Text("Name", e => e.Name),
        CrudColumn<ContactMessage>.Text("Phone", e => e.Phone),
        CrudColumn<ContactMessage>.Text("Message", e => e.Message),
        CrudColumn<ContactMessage>.Date("Received", e => e.CreatedAt),
        CrudColumn<ContactMessage>.Bool("Read", e => e.IsRead)
    };

    protected override async Task OnOpenAsync(ContactMessage model)
    {
        if (!model.IsRead)
        {
            model.IsRead = true;
            await Crud.UpdateAsync(model);
        }
    }
}
