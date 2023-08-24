using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiodOdStaniula.Models;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    [Authorize]
    public class EditProductController : BaseController
    {
        private readonly IEditProductService _editProductService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger<IEditProductService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditProductController
            (
                DbStoreContext context,
                IEditProductService editProductService,
                ICartService cartService,
                ICheckoutService checkoutService,
                ICustomerService customerService,
                ITotalCostService totalCostService,
                IFileUploadService fileUploadService,
                ILogger<IEditProductService> logger,
                IWarehouseService warehouseService,
                IWebHostEnvironment webHostEnvironment
            ) : base(context, cartService, checkoutService, customerService, totalCostService)
        {
            _editProductService = editProductService;
            _fileUploadService = fileUploadService;
            _logger = logger;
            _warehouseService = warehouseService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Product? product = await _warehouseService.GetProductAsync(id)!;

            if (product == null)
            {
                return View("_NotFound");
            }
            else
            {
                ProductViewModel productViewModel = await PrepareProductViewModel(product);
                return View(productViewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ProductId, ProductViewModel model)
        {
            Product? product = await _warehouseService.GetProductAsync(ProductId)!;

            if (product == null || ProductId != product.ProductId)
            {
                return View("_NotFound");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }


            // If a new photo was uploaded.
            if (model.ProductImages != null && model.ProductImages.Any())
            {
                var imagePaths = await _fileUploadService.UploadFilesAsync(model.ProductImages);

                product.ProductImages = product.ProductImages ?? new List<ProductImage>();
                foreach (var path in imagePaths)
                {
                    product.ProductImages.Add(new ProductImage { ImagePath = path, ProductId = product.ProductId });
                }
            }

            product = new Product
            {
                ProductId = model.ProductId,
                Priority = model.Priority,
                CategoryId = model.CategoryId,
                Name = model.Name,
                Price = model.Price,
                Weight = model.Weight,
                Description = model.Description,
                AmountAvailable = model.AmountAvailable,
            };

            var (IsSuccess, Error) = await _editProductService.UpdateProductAsync(product);

            if (IsSuccess)
            {
                return RedirectToAction("Details", "Products", new { id = product?.ProductId });
            }

            if (Error != null)
            {
                return HandleUpdateError(Error, product);
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId, int ProductId)
        {
            Product? product = await _warehouseService.GetProductAsync(ProductId)!;

            if (_context.ProductImages != null)
            {
                var image = await _context.ProductImages.FindAsync(imageId);
                if (image == null || string.IsNullOrEmpty(image.ImagePath))
                {
                    return View("_NotFound");
                }

                var fileName = Path.GetFileName(image.ImagePath);
                if (string.IsNullOrEmpty(fileName))
                {
                    return View("_NotFound");
                }

                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                _context.ProductImages.Remove(image);
                await _context.SaveChangesAsync();

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                else
                {
                    return View("_NotFound");
                }
            }
            return RedirectToAction("Edit", "EditProduct", new { id = product?.ProductId });
        }

        private IActionResult HandleUpdateError(Exception ex, Product product)
        {
            _logger.LogError(ex, "Wystąpił błąd podczas aktualizacji produktu. Spróbuj ponownie później.");
            ModelState.AddModelError(string.Empty, "Wystąpił błąd podczas aktualizacji produktu. Spróbuj ponownie później.");
            return View(product);
        }

    }
}
