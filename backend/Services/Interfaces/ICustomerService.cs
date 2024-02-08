using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<UserDto?> GetUserAsync(string userId);
    }
}