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

        public async Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                if (_context.Products != null)
                {
                    var products = await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.ProductImages)
                        .ToListAsync();

                    var productDtos = products.Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Price = p.Price,
                        Weight = p.Weight,
                        AmountAvailable = p.AmountAvailable,
                        // ... inne właściwości ...
                        ProductImages = p.ProductImages!.Select(pi => new ProductImageDto
                        {
                            ImageId = pi.ImageId,
                            ImagePath = pi.ImagePath
                        }).ToList()
                    }).ToList();

                    return new ServiceResult<IEnumerable<ProductDto>>
                    {
                        Data = productDtos,
                        Success = true
                    };
                }
                else
                {
                    return new ServiceResult<IEnumerable<ProductDto>>
                    {
                        Success = false
                    };
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Błąd podczas pobierania produktów");

                return new ServiceResult<IEnumerable<ProductDto>>
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
