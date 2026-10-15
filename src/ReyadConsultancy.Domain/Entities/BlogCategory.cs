using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class BlogCategory : BaseEntity
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Slug { get; set; }

    public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();

}
