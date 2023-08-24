using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DbStoreContext _context;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(DbStoreContext context, ILogger<CustomerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Customer> AddNewCustomer(Customer customer)
        {
            _context.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> GetCustomerAsync(Guid id)
        {
            if (_context.Customers != null)
            {
                var customer = await _context.Customers
                    .Include(p => p.Orders)
                    .FirstOrDefaultAsync(p => p.CustomerId == id);

                if (customer == null)
                {
                    _logger.LogError("Nie znaleziono klienta");
                }

                return customer;
            }
            _logger.LogError("Nie znaleziono klienta");
            return null;
        }
    }
}