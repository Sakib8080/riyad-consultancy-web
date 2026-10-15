using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;
using ReyadConsultancy.Web.Areas.Admin.Models;


namespace ReyadConsultancy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class TestimonialsController : CrudController<Testimonial>
{
    public TestimonialsController(ICrudService<Testimonial> crud, IMediaService media) : base(crud, media)
    {
    }

    protected override string EntityTitle => "Testimonial";
    protected override string ListTitle => "Testimonials";

    protected override string[] ImageProperties => new[] { "PhotoUrl" };

    protected override IReadOnlyList<CrudColumn<Testimonial>> Columns => new[]
    {
        CrudColumn<Testimonial>.Image("Photo", e => e.PhotoUrl),
        CrudColumn<Testimonial>.Text("Name", e => e.Name),
        CrudColumn<Testimonial>.Text("Message", e => e.Message),
        CrudColumn<Testimonial>.Text("Order", e => e.SortOrder),
        CrudColumn<Testimonial>.Bool("Active", e => e.IsActive)
    };

}
