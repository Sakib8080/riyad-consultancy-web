using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class Testimonial : BaseEntity, ISortable
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Designation { get; set; }

    [Required, StringLength(1500)]
    public string Message { get; set; } = string.Empty;

    [StringLength(500)]
    public string? PhotoUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

}
