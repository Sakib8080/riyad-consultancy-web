using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class Slide : BaseEntity, ISortable
{
    [StringLength(200)]
    public string? Subtitle { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [StringLength(60)]
    public string? ButtonText { get; set; }

    [StringLength(300)]
    public string? ButtonUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

}
