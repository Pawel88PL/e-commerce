using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DbStoreContext _context;
        private readonly ILogger<ProductService> _logger;

        public CustomerService(DbStoreContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserDto?> GetUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                Email = user.Email!,
                Name = user.Name!,
                Surname = user.Surname!,
                City = user.City!,
                Street = user.Street!,
                Address = user.Address!,
                PostalCode = user.PostalCode!
            };
        }

    }
}