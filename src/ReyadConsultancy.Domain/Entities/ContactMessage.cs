using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class ContactMessage : BaseEntity
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }

    [StringLength(3000)]
    public string? Message { get; set; }

    public bool IsRead { get; set; } = false;

}
