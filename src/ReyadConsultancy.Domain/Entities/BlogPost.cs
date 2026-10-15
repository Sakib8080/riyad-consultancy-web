using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class BlogPost : BaseEntity
{
    [Required, StringLength(250)]
    public string Title { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Slug { get; set; }

    [StringLength(600)]
    public string? Summary { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [StringLength(100)]
    public string? Author { get; set; }

    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    [StringLength(200)]
    public string? Tags { get; set; }

    public int? CategoryId { get; set; }

    public bool IsPublished { get; set; } = true;


    public BlogCategory? Category { get; set; }
    public ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();

}
