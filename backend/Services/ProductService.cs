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

        public async Task<ServiceResult<ProductDto>> AddAsync(ProductDto productDto)
        {
            try
            {
                Product product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Weight = productDto.Weight,
                    AmountAvailable = productDto.AmountAvailable,
                    Popularity = productDto.Popularity,
                    Priority = productDto.Priority,
                    DateAdded = DateTime.Now,
                    CategoryId = productDto.CategoryId,
                };
                _context.Add(product);
                await _context.SaveChangesAsync();

                if (productDto.ImagePaths != null && productDto.ImagePaths.Any())
                {
                    product.ProductImages = new List<ProductImage>();
                    foreach (var path in productDto.ImagePaths)
                    {
                        product.ProductImages!.Add(new ProductImage { ImagePath = path, ProductId = product.ProductId });
                    }
                    await _context.SaveChangesAsync();
                }

                productDto.ProductId = product.ProductId;

                return new ServiceResult<ProductDto>
                {
                    Success = true,
                    Data = productDto
                };
            }

            catch (Exception ex)
            {
                return new ServiceResult<ProductDto>
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

        public async Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products!
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .ToListAsync();

                var productDtos = products.Select(MapToProductDto).ToList();

                return new ServiceResult<IEnumerable<ProductDto>>
                {
                    Data = productDtos,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return HandleError<IEnumerable<ProductDto>>(ex, "Błąd podczas pobierania produktów");
            }
        }

        public async Task<ServiceResult<PaginatedList<ProductDto>>> GetAllProductsAsync(int page, int itemsPerPage)
        {
            try
            {
                var query = _context.Products!
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .OrderBy(p => p.ProductId);

                var totalItems = await query.CountAsync();
                var products = await query.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

                var productDtos = products.Select(MapToProductDto).ToList();

                var paginatedData = new PaginatedList<ProductDto>(productDtos, totalItems, page, itemsPerPage);

                return new ServiceResult<PaginatedList<ProductDto>>
                {
                    Data = paginatedData,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return HandleError<PaginatedList<ProductDto>>(ex, "Błąd podczas pobierania produktów.");
            }
        }


        public async Task<ServiceResult<ProductDto>> GetProductAsync(int id)
        {
            try
            {
                var product = await _context.Products!
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    return new ServiceResult<ProductDto>
                    {
                        Success = false,
                        ErrorMessage = "Nie znaleziono produktu."
                    };
                }

                var productDto = MapToProductDto(product);

                return new ServiceResult<ProductDto>
                {
                    Data = productDto,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return HandleError<ProductDto>(ex, "Błąd podczas pobierania produktu.");
            }
        }

        public async Task<ServiceResult<ProductDto>> UpdateAsync(int id, ProductDto updatedProduct)
        {
            try
            {
                var product = await _context.Products!.FindAsync(id);
                if (product == null)
                {
                    return new ServiceResult<ProductDto>
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

                if (updatedProduct.ImagePaths != null && updatedProduct.ImagePaths.Any())
                {
                    product.ProductImages = new List<ProductImage>();
                    foreach (var path in updatedProduct.ImagePaths)
                    {
                        product.ProductImages!.Add(new ProductImage { ImagePath = path, ProductId = product!.ProductId });
                        await _context.SaveChangesAsync();
                    }
                }

                updatedProduct.ProductId = product.ProductId;
                return new ServiceResult<ProductDto>
                {
                    Success = true,
                    Data = updatedProduct
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<ProductDto>
                {
                    Success = false,
                    ErrorMessage = $"Wystąpił błąd podczas aktualizacji produktu: {ex.Message}"
                };
            }
        }

        private ProductDto MapToProductDto(Product product)
        {
            return new ProductDto
            {
                AmountAvailable = product.AmountAvailable,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                DateAdded = product.DateAdded,
                Description = product.Description,
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
                ImagePath = productImage.ImagePath,
                ProductId = productImage.ProductId,
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