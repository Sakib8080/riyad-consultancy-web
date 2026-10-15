using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class Counter : BaseEntity, ISortable
{
    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    public int Value { get; set; }

    [StringLength(500)]
    public string? IconUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

}
