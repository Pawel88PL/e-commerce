using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class AddProductService: IAddProductService
    {

        private readonly DbStoreContext _context;

        public AddProductService(DbStoreContext context)
        {
            _context = context;
        }

        public async Task<Product> AddNewProductAsync(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
