using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;

namespace ReyadConsultancy.Domain.Entities;

/// <summary>Single-row table holding global website settings.</summary>
public class SiteSetting : BaseEntity
{
    [Required, StringLength(150)]
    public string SiteName { get; set; } = "Reyad Business Solution and Consultancy";

    [StringLength(50)]
    public string? ShortName { get; set; } = "RBSC";

    [StringLength(250)]
    public string? Tagline { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? LogoUrl { get; set; }

    [StringLength(500)]
    public string? FaviconUrl { get; set; }

    [StringLength(500)]
    public string? FooterImageUrl { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Phone1 { get; set; }

    [StringLength(50)]
    public string? Phone2 { get; set; }

    [StringLength(150)]
    public string? Email1 { get; set; }

    [StringLength(150)]
    public string? Email2 { get; set; }

    [StringLength(500)]
    public string? FooterAboutText { get; set; }

    [StringLength(300)]
    public string? NewsletterText { get; set; }

    [StringLength(300)]
    public string? FacebookUrl { get; set; }

    [StringLength(300)]
    public string? TwitterUrl { get; set; }

    [StringLength(300)]
    public string? LinkedInUrl { get; set; }

    [StringLength(300)]
    public string? PinterestUrl { get; set; }

    [StringLength(200)]
    public string? CopyrightText { get; set; }

    public double? MapLatitude { get; set; }
    public double? MapLongitude { get; set; }

    [StringLength(200)]
    public string? GoogleMapsApiKey { get; set; }
}
