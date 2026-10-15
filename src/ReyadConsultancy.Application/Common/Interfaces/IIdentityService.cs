namespace ReyadConsultancy.Application.Common.Interfaces;

public record AuthResult(bool Succeeded, IReadOnlyList<string> Errors, bool PendingApproval = false)
{
    public static AuthResult Ok(bool pendingApproval = false) => new AuthResult(true, Array.Empty<string>(), pendingApproval);
    public static AuthResult Fail(params string[] errors) => new AuthResult(false, errors);
}

public record UserDto(string Id, string FullName, string Email, bool IsApproved, bool IsAdmin, DateTime CreatedAt);

public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(string fullName, string email, string password);
    Task<AuthResult> LoginAsync(string email, string password, bool rememberMe);
    Task LogoutAsync();
    Task<AuthResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto?> GetUserAsync(string userId);
    Task<AuthResult> SetApprovalAsync(string userId, bool approved, string actingUserId);
    Task<AuthResult> DeleteUserAsync(string userId, string actingUserId);
}
