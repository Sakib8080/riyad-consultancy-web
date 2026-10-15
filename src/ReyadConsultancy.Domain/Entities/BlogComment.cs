using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class BlogComment : BaseEntity
{
    public int BlogPostId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Phone { get; set; }

    [Required, StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    public bool IsApproved { get; set; } = false;

    public BlogPost? BlogPost { get; set; }

}
