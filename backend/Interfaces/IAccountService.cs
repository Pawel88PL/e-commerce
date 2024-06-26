using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel?> FindByIdAsync(string userId);
        Task<UserModel?> FindByEmailAsync(string email);
        Task<IdentityResult> ConfirmEmailAsync(UserModel user, string token);
        Task<IdentityResult> CreateAsync(UserModel user, string password);
        Task<bool> PasswordSignInAsync(string email, string password);
        Task SignOutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(UserModel user);
        Task AddToRoleAsync(UserModel user, string role);
        Task<IList<string>> GetRolesAsync(UserModel user);
        string GenerateJwtTokenForUser(UserModel user);
    }
}