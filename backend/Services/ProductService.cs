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

        public async Task<ServiceResult<ProductAddDto>> AddAsync(ProductAddDto productAddDto)
        {
            try
            {
                Product product = new Product
                {
                    Name = productAddDto.Name,
                    Description = productAddDto.Description,
                    Price = productAddDto.Price,
                    Weight = productAddDto.Weight,
                    AmountAvailable = productAddDto.AmountAvailable,
                    Popularity = productAddDto.Popularity,
                    Priority = productAddDto.Priority,
                    DateAdded = DateTime.Now,
                    CategoryId = productAddDto.CategoryId,
                };

                if (productAddDto.ImagePaths != null && productAddDto.ImagePaths.Any())
                {
                    product.ProductImages = new List<ProductImage>();
                    foreach (var path in productAddDto.ImagePaths)
                    {
                        product.ProductImages!.Add(new ProductImage { ImagePath = path, ProductId = productAddDto!.ProductId });
                    }
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                productAddDto.ProductId = product.ProductId;

                return new ServiceResult<ProductAddDto>
                {
                    Success = true,
                    Data = productAddDto
                };
            }

            catch (Exception ex)
            {
                return new ServiceResult<ProductAddDto>
                {
                    Success = false,
                    ErrorMessage = $"Wystąpił błąd podczas dodawania produktu: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var product = await _context.Products!.FindAsync(id);
                if (product == null)
                {
                    return new ServiceResult<bool>
                    {
                        Success = false,
                        ErrorMessage = "Nie znaleziono produktu."
                    };
                }
                else
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();

                    return new ServiceResult<bool>
                    {
                        Success = true,
                    };

                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>
                {
                    Success = false,
                    ErrorMessage = $"Wystąpił błąd podczas usuwania produktu: {ex.Message}"
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

        public async Task<ServiceResult<Product>> UpdateAsync(int id, Product updatedProduct)
        {
            try
            {
                var product = await _context.Products!.FindAsync(id);
                if (product == null)
                {
                    return new ServiceResult<Product>
                    {
                        Success = false,
                        ErrorMessage = "Nie znaleziono produktu."
                    };
                }

                product.AmountAvailable = updatedProduct.AmountAvailable;
                product.CategoryId = updatedProduct.CategoryId;
                product.Description = updatedProduct.Description;
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
                product.Priority = updatedProduct.Priority;
                product.Weight = updatedProduct.Weight;

                _context.Products.Update(product);
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
                    ErrorMessage = $"Wystąpił błąd podczas aktualizacji produktu: {ex.Message}"
                };
            }
        }


        private ProductDisplayDto MapToProductDisplayDto(Product product)
        {
            return new ProductDisplayDto
            {
                AmountAvailable = product.AmountAvailable,
                Category = product.Category?.Name,
                CategoryId = product.CategoryId,
                Description = product.Description,
                DateAdded = product.DateAdded,
                Name = product.Name,
                Price = product.Price,
                Priority = product.Priority,
                ProductId = product.ProductId,
                ProductImages = product.ProductImages!.Select(MapToProductImageDto).ToList(),
                Weight = product.Weight
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