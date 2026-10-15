using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class TeamMember : BaseEntity, ISortable
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Designation { get; set; }

    [StringLength(500)]
    public string? PhotoUrl { get; set; }

    [StringLength(300)]
    public string? FacebookUrl { get; set; }

    [StringLength(300)]
    public string? TwitterUrl { get; set; }

    [StringLength(300)]
    public string? LinkedInUrl { get; set; }

    [StringLength(300)]
    public string? PinterestUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

}
