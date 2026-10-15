using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class Service : BaseEntity, ISortable
{
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Slug { get; set; }

    [StringLength(150)]
    public string? Subtitle { get; set; }

    [StringLength(600)]
    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [StringLength(500)]
    public string? IconUrl { get; set; }

    [StringLength(500)]
    public string? BannerImageUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

}
