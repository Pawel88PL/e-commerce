using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Services;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var products = await _warehouseService.GetAllProductsAsync();

            return View(products);
        }
    }
}
