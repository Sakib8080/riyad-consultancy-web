using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class AboutHighlight : BaseEntity, ISortable
{
    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(60)]
    public string IconClass { get; set; } = "icon icon-Bulb";

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

}
