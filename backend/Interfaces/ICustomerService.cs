using backend.Models;

namespace backend.Interfaces
{
    public interface ICustomerService
    {
        Task<UserDto?> GetUserAsync(string userId);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(string userId, UserDto userDto);
        Task<bool> UpdateGuestUserAsync(string userId, Register registerData);
    }
}