using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReyadConsultancy.Application.Common.Interfaces;

namespace ReyadConsultancy.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    public const string AdminRole = "Admin";

    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;

    public IdentityService(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn)
    {
        _users = users;
        _signIn = signIn;
    }

    public async Task<AuthResult> RegisterAsync(string fullName, string email, string password)
    {
        email = email.Trim();
        if (await _users.FindByEmailAsync(email) != null)
            return AuthResult.Fail("An account with this email already exists.");

        // The very first account becomes the administrator. Everyone else waits for approval,
        // otherwise anybody on the internet could register and edit the website.
        var isFirst = !await _users.Users.AnyAsync();
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName.Trim(),
            EmailConfirmed = true,
            IsApproved = isFirst,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _users.CreateAsync(user, password);
        if (!result.Succeeded) return AuthResult.Fail(result.Errors.Select(e => e.Description).ToArray());

        if (isFirst) await _users.AddToRoleAsync(user, AdminRole);
        return AuthResult.Ok(pendingApproval: !isFirst);
    }

    public async Task<AuthResult> LoginAsync(string email, string password, bool rememberMe)
    {
        var user = await _users.FindByEmailAsync(email.Trim());
        if (user == null) return AuthResult.Fail("Invalid email or password.");

        var check = await _signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (check.IsLockedOut) return AuthResult.Fail("This account is temporarily locked. Please try again later.");
        if (!check.Succeeded) return AuthResult.Fail("Invalid email or password.");
        if (!user.IsApproved) return AuthResult.Fail("Your account is waiting for administrator approval.");

        await _signIn.SignInAsync(user, rememberMe);
        return AuthResult.Ok();
    }

    public Task LogoutAsync() => _signIn.SignOutAsync();

    public async Task<AuthResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _users.FindByIdAsync(userId);
        if (user == null) return AuthResult.Fail("User not found.");
        var result = await _users.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded) return AuthResult.Fail(result.Errors.Select(e => e.Description).ToArray());
        await _signIn.RefreshSignInAsync(user);
        return AuthResult.Ok();
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _users.Users.OrderBy(u => u.CreatedAt).ToListAsync();
        var list = new List<UserDto>();
        foreach (var u in users)
            list.Add(new UserDto(u.Id, u.FullName, u.Email ?? "", u.IsApproved, await _users.IsInRoleAsync(u, AdminRole), u.CreatedAt));
        return list;
    }

    public async Task<UserDto?> GetUserAsync(string userId)
    {
        var u = await _users.FindByIdAsync(userId);
        return u == null ? null : new UserDto(u.Id, u.FullName, u.Email ?? "", u.IsApproved, await _users.IsInRoleAsync(u, AdminRole), u.CreatedAt);
    }

    public async Task<AuthResult> SetApprovalAsync(string userId, bool approved, string actingUserId)
    {
        var user = await _users.FindByIdAsync(userId);
        if (user == null) return AuthResult.Fail("User not found.");
        if (!approved && userId == actingUserId) return AuthResult.Fail("You cannot revoke your own access.");

        user.IsApproved = approved;
        var update = await _users.UpdateAsync(user);
        if (!update.Succeeded) return AuthResult.Fail(update.Errors.Select(e => e.Description).ToArray());

        if (approved && !await _users.IsInRoleAsync(user, AdminRole)) await _users.AddToRoleAsync(user, AdminRole);
        if (!approved && await _users.IsInRoleAsync(user, AdminRole)) await _users.RemoveFromRoleAsync(user, AdminRole);
        return AuthResult.Ok();
    }

    public async Task<AuthResult> DeleteUserAsync(string userId, string actingUserId)
    {
        if (userId == actingUserId) return AuthResult.Fail("You cannot delete your own account.");
        var user = await _users.FindByIdAsync(userId);
        if (user == null) return AuthResult.Fail("User not found.");
        var result = await _users.DeleteAsync(user);
        return result.Succeeded ? AuthResult.Ok() : AuthResult.Fail(result.Errors.Select(e => e.Description).ToArray());
    }
}
