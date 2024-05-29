using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DbStoreContext _context;
        private readonly ILogger<CustomerService> _logger;
        private readonly UserManager<UserModel> _userManager;

        public CustomerService(DbStoreContext context, ILogger<CustomerService> logger, UserManager<UserModel> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                return await _userManager.Users
                .Select(u => new UserDto
                {
                    Email = u.Email ?? string.Empty,
                    Name = u.Name ?? string.Empty,
                    Surname = u.Surname ?? string.Empty,
                    City = u.City ?? string.Empty,
                    Street = u.Street ?? string.Empty,
                    Address = u.Address ?? string.Empty,
                    PostalCode = u.PostalCode ?? string.Empty,
                    PhoneNumber = u.PhoneNumber ?? string.Empty,
                    RegistrationDate = u.RegistrationDate,
                    EmailConfirmed = u.EmailConfirmed
                })
                .OrderByDescending(u => u.RegistrationDate)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                return new List<UserDto>();
            }
        }


        public async Task<UserDto?> GetUserAsync(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new UserDto
                    {
                        Email = u.Email ?? string.Empty,
                        Name = u.Name ?? string.Empty,
                        Surname = u.Surname ?? string.Empty,
                        City = u.City ?? string.Empty,
                        Street = u.Street ?? string.Empty,
                        Address = u.Address ?? string.Empty,
                        PostalCode = u.PostalCode ?? string.Empty,
                        PhoneNumber = u.PhoneNumber ?? string.Empty
                    })
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching user with ID {userId}");
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(string userId, UserDto userDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.Address = userDto.Address;
                user.City = userDto.City;
                user.Email = userDto.Email;
                user.Name = userDto.Name;
                user.PostalCode = userDto.PostalCode;
                user.PhoneNumber = userDto.PhoneNumber;
                user.Surname = userDto.Surname;
                user.Street = userDto.Street;
                user.UserName = userDto.Email;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {userId}");
                return false;
            }
        }
    }
}
