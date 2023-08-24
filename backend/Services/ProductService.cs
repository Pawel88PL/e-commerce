using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Services
{
    public class ProductService : IProductService
    {

        public List<Product> Sort(IEnumerable<Product> products, string sortOrder)
        {
            return sortOrder switch
            {
                "category" => products.OrderBy(p => p.CategoryId).ToList(),
                "name_asc" => products.OrderBy(p => p.Name).ToList(),
                "price_asc" => products.OrderBy(p => p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.Price).ToList(),
                "date_asc" => products.OrderBy(p => p.DateAdded).ToList(),
                "available-desc" => products.OrderByDescending(p => p.AmountAvailable).ToList(),
                "popularity" => products.OrderByDescending(p => p.Popularity).ToList(),
                _ => products.OrderBy(p => p.Priority).ToList(),
            };
        }

        public List<Product> Filter(IEnumerable<Product> products, string filterCondition)
        {
            if (!string.IsNullOrEmpty(filterCondition))
            {
                if (products.Any(p => p.Category != null && p.Category.Name != null && p.Category.Name.Equals(filterCondition)))
                {
                    return products.Where(p => p.Category != null && p.Category.Name != null && p.Category.Name.Equals(filterCondition)).ToList();
                }
            }
            return products.ToList();
        }
    }
}
