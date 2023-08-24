using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class DeleteService : IDeleteService
    {
        private readonly DbStoreContext _context;
        private readonly ILogger<DeleteService> _logger;

        public DeleteService(DbStoreContext context, ILogger<DeleteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> DeleteProductAsync(int ProductId)
        {
            if (_context.Products != null)
            {
                var product = await _context.Products.FindAsync(ProductId);
                if (product == null)
                {
                    _logger.LogError("Nie znaleziono produktu o Id: {ProductId}", ProductId);
                    return false;
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usunięto produkt o Id: {ProductId}", ProductId);
                return true;
            }
            return false;
        }
    }
}