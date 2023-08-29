using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class AddNewProductService: IAddNewProductService
    {
        private readonly DbStoreContext _context;

        public AddNewProductService(DbStoreContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<Product>> AddNewProductAsync(Product product, List<string> imagePaths)
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
    }
}
