using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class Faq : BaseEntity, ISortable
{
    [Required, StringLength(300)]
    public string Question { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Answer { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

}
