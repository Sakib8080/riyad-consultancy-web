using System.ComponentModel.DataAnnotations;

namespace ReyadConsultancy.Web.Models;

public class ContactForm
{
    [Required, StringLength(100)] public string Name { get; set; } = string.Empty;
    [Required, StringLength(50)] public string Phone { get; set; } = string.Empty;
    [EmailAddress, StringLength(150)] public string? Email { get; set; }
    [StringLength(300)] public string? Address { get; set; }
    [Required, StringLength(3000)] public string Message { get; set; } = string.Empty;
    /// <summary>Honeypot field - real visitors never fill this in.</summary>
    public string? Website { get; set; }
}

public class AppointmentForm
{
    [Required, StringLength(100)] public string FirstName { get; set; } = string.Empty;
    [StringLength(100)] public string? LastName { get; set; }
    [Required, StringLength(50)] public string Phone { get; set; } = string.Empty;
    [EmailAddress, StringLength(150)] public string? Email { get; set; }
    public DateTime? AppointmentDate { get; set; }
    [StringLength(50)] public string? AppointmentTime { get; set; }
    [StringLength(50)] public string? CustomerId { get; set; }
    [StringLength(100)] public string? CaseType { get; set; }
    [StringLength(20)] public string? Gender { get; set; }
    [StringLength(3000)] public string? Message { get; set; }
    public string? Website { get; set; }
}

public class CommentForm
{
    [Required, StringLength(100)] public string Name { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = string.Empty;
    [StringLength(50)] public string? Phone { get; set; }
    [Required, StringLength(2000)] public string Message { get; set; } = string.Empty;
    public string? Website { get; set; }
}
