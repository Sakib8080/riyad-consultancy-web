using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class Appointment : BaseEntity
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? LastName { get; set; }

    [Required, StringLength(50)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(150)]
    public string? Email { get; set; }

    public DateTime? AppointmentDate { get; set; }

    [StringLength(50)]
    public string? AppointmentTime { get; set; }

    [StringLength(50)]
    public string? CustomerId { get; set; }

    [StringLength(100)]
    public string? CaseType { get; set; }

    [StringLength(20)]
    public string? Gender { get; set; }

    [StringLength(3000)]
    public string? Message { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    [StringLength(2000)]
    public string? AdminNotes { get; set; }

}
