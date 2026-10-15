using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class ClientLogo : BaseEntity, ISortable
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [StringLength(300)]
    public string? LinkUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

}
