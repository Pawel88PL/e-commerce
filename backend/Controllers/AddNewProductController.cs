using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace MiodOdStaniula.Controllers
{
    public class AddNewProductController : Controller
    {
        private readonly IAddProductService _addProductService;
        private readonly IEditProductService _editProductService;
        private readonly IFileUploadService _fileUploadService;

        public AddNewProductController(IAddProductService addProductService,
            IEditProductService editProductService, IFileUploadService fileUploadService)
        {
            _addProductService = addProductService;
            _editProductService = editProductService;
            _fileUploadService = fileUploadService;
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            var product = new Product
            {
                Name = productViewModel.Name,
                Priority = productViewModel.Priority,
                Description = productViewModel.Description,
                Price = productViewModel.Price,
                Weight = productViewModel.Weight,
                AmountAvailable = productViewModel.AmountAvailable,
                DateAdded = DateTime.Today,
                CategoryId = productViewModel.CategoryId,
                ProductImages = new List<ProductImage>()
            };

            var addedProduct = await _addProductService.AddNewProductAsync(product);


            if (productViewModel.ProductImages != null && productViewModel.ProductImages.Any())
            {
                var imagePaths = await _fileUploadService.UploadFilesAsync(productViewModel.ProductImages);

                foreach (var path in imagePaths)
                {
                    addedProduct?.ProductImages?.Add(new ProductImage { ImagePath = path, ProductId = addedProduct.ProductId });
                }

                // Aktualizacja produktu z nowymi zdjęciami (jeśli Twój serwis tego wymaga)
                await _editProductService.UpdateProductAsync(addedProduct);
            }
            return RedirectToAction("Index", "Warehouse");
        }

    }
}