using Microsoft.AspNetCore.Identity;

namespace ReyadConsultancy.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    /// <summary>New registrations stay inactive until an administrator approves them.</summary>
    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
