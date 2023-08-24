using Microsoft.EntityFrameworkCore;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly DbStoreContext _context;
        private readonly ILogger<WarehouseService> _logger;

        public WarehouseService(DbStoreContext context, ILogger<WarehouseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Product?> GetProductAsync(int ProductId)
        {
            if (_context.Products != null)
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == ProductId);

                if (product == null)
                {
                    _logger.LogError($"Nie znaleziono produktu o ID: {ProductId}", ProductId);
                }

                return product;
            }
            return null;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            if (_context.Products != null)
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .ToListAsync();

                return products;
            }
            return new List<Product>();
        }
    }
}
