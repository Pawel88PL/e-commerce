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

        public async Task<ServiceResult<Product>> AddAsync(Product product, List<string> imagePaths)
        {
            try
            {
                if (imagePaths != null && imagePaths.Any())
                {
                    foreach (var path in imagePaths)
                    {
                        product.ProductImages!.Add(new ProductImage { ImagePath = path });
                    }
                }

                _context.Add(product);
                await _context.SaveChangesAsync();

                return new ServiceResult<Product>
                {
                    Success = true,
                    Data = product
                };
            }

            catch (Exception ex)
            {
                return new ServiceResult<Product>
                {
                    Success = false,
                    ErrorMessage = $"Błąd podczas dodawania produktu: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResult<IEnumerable<ProductDisplayDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products!
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .ToListAsync();

                var productDtos = products.Select(MapToProductDisplayDto).ToList();

                return new ServiceResult<IEnumerable<ProductDisplayDto>>
                {
                    Data = productDtos,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return HandleError<IEnumerable<ProductDisplayDto>>(ex, "Błąd podczas pobierania produktów");
            }
        }

        public async Task<ServiceResult<ProductDisplayDto>> GetProductAsync(int id)
        {
            try
            {
                var product = await _context.Products!
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    return new ServiceResult<ProductDisplayDto>
                    {
                        Success = false,
                        ErrorMessage = "Nie znaleziono produktu."
                    };
                }

                var productDto = MapToProductDisplayDto(product);

                return new ServiceResult<ProductDisplayDto>
                {
                    Data = productDto,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return HandleError<ProductDisplayDto>(ex, "Błąd podczas pobierania produktu.");
            }
        }

        private ProductDisplayDto MapToProductDisplayDto(Product product)
        {
            return new ProductDisplayDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Weight = product.Weight,
                AmountAvailable = product.AmountAvailable,
                CategoryId = product.CategoryId,
                Category = product.Category?.Name,
                ProductImages = product.ProductImages!.Select(MapToProductImageDto).ToList()
            };
        }

        private ProductImageDto MapToProductImageDto(ProductImage productImage)
        {
            return new ProductImageDto
            {
                ImageId = productImage.ImageId,
                ImagePath = productImage.ImagePath
            };
        }

        private ServiceResult<T> HandleError<T>(Exception ex, string logMessage)
        {
            _logger.LogError(ex, logMessage);

            return new ServiceResult<T>
            {
                Success = false,
                ErrorMessage = "Wystąpił problem podczas pobierania produktów. Spróbuj ponownie później."
            };
        }

    }

    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

}