using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class TotalCostService : ITotalCostService
    {
        private readonly DbStoreContext _context;

        public TotalCostService(DbStoreContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateShippingCostAsync(Guid cartId)
        {
            if (_context.CartItem != null)
            {
                var cartItems = await _context.CartItem.Where(item => item.ShopingCartId == cartId)
                    .Include(item => item.Product)
                    .ToListAsync();

                int totalItems = cartItems.Sum(item => item.Quantity);

                if (totalItems > 0 && totalItems <= 6)
                {
                    return 18.99m;
                }
                else if (totalItems > 6 && totalItems <= 10)
                {
                    return 20.99m;
                }
                else if (totalItems > 10)
                {
                    return 36.99m;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        public async Task<decimal> CalculateProductCostAsync(Guid cartId)
        {
            if (_context.CartItem != null)
            {
                var cartItems = await _context.CartItem.Where(item => item.ShopingCartId == cartId)
                    .Include(item => item.Product)
                    .ToListAsync();

                decimal productCost = cartItems.Sum(item => item.Product?.Price * item.Quantity) ?? 0;

                return productCost;
            }
            return 0;
        }


        public async Task<decimal> CalculateTotalAsync(Guid cartId)
        {
            decimal shippingCost = await CalculateShippingCostAsync(cartId);
            decimal productCost = await CalculateProductCostAsync(cartId);

            return productCost + shippingCost;
        }
    }
}
