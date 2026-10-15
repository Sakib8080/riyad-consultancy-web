using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class PageContent : BaseEntity
{
    [Required, StringLength(100)]
    public string Key { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Title { get; set; }

    public string? Body { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

}
