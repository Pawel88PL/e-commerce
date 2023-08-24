using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class ProductService : IProductService
    {
        private readonly DbStoreContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(DbStoreContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            try
            {
                if (_context.Products != null)
                {
                    var products = await _context.Products.ToListAsync();
                    return new ServiceResult<IEnumerable<Product>>
                    {
                        Data = products,
                        Success = true
                    };
                }
                else
                {
                    return new ServiceResult<IEnumerable<Product>>
                    {
                        Success = false
                    };
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Błąd podczas pobierania produktów");

                return new ServiceResult<IEnumerable<Product>>
                {
                    Success = false,
                    ErrorMessage = "Wystąpił problem podczas pobierania produktów. Spróbuj ponownie później."
                };
            }
        }
    }

    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

}
