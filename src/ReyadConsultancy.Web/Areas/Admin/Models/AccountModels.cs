using System.ComponentModel.DataAnnotations;

namespace ReyadConsultancy.Web.Areas.Admin.Models;

public class LoginViewModel
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}

public class RegisterViewModel
{
    [Required, StringLength(100), Display(Name = "Full name")] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = string.Empty;
    [Required, StringLength(100, MinimumLength = 8), DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "The passwords do not match."), Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password), Display(Name = "Current password")] public string CurrentPassword { get; set; } = string.Empty;
    [Required, StringLength(100, MinimumLength = 8), DataType(DataType.Password), Display(Name = "New password")] public string NewPassword { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), Compare(nameof(NewPassword), ErrorMessage = "The passwords do not match."), Display(Name = "Confirm new password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
