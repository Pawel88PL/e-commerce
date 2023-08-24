using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class EditProductService : IEditProductService
    {
        private readonly DbStoreContext _context;
        private readonly ILogger<EditProductService> _logger;

        public EditProductService(DbStoreContext context, ILogger<EditProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, Exception? Error)> UpdateProductAsync(Product product)
        {
            try
            {
                if (_context.Products != null)
                {
                    var productInDb = await _context.Products
                        .Include(p => p.ProductImages)
                        .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

                    if (productInDb != null)
                    {
                        productInDb.Priority = product.Priority;
                        productInDb.Name = product.Name;
                        productInDb.Price = product.Price;
                        productInDb.Weight = product.Weight;
                        productInDb.Description = product.Description;
                        productInDb.AmountAvailable = product.AmountAvailable;
                        productInDb.CategoryId = product.CategoryId;

                        if (product.ProductImages != null)
                        {
                            foreach (var image in product.ProductImages)
                            {
                                if (productInDb.ProductImages != null)
                                {
                                    if (!productInDb.ProductImages.Any(i => i.ImageId == image.ImageId))
                                    {
                                        productInDb.ProductImages.Add(image);
                                    }
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                        return (true, null);

                    }
                    else
                    {

                        return (false, new InvalidOperationException($"Nie znaleziono produktu o ID: {product.ProductId}"));
                    }
                }
                return (false, new InvalidOperationException($"Nie znaleziono produktu o ID: {product.ProductId}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas aktualizacji danych pacjenta o Id: {Id}", product.ProductId);
                return (false, ex);
            }
        }
    }
}