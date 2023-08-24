using MiodOdStaniula.Models;

namespace MiodOdStaniula.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> Sort(IEnumerable<Product> products, string sortOrder);
        List<Product> Filter(IEnumerable<Product> products, string filterCondition);

    }
}
