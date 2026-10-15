using System.ComponentModel.DataAnnotations;
using ReyadConsultancy.Domain.Common;
using ReyadConsultancy.Domain.Enums;

namespace ReyadConsultancy.Domain.Entities;

public class NewsletterSubscriber : BaseEntity
{
    [Required, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

}
