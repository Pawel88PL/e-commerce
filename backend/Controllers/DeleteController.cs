using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Services.Interfaces;

namespace MiodOdStaniula.Controllers
{
    public class DeleteController : Controller
    {
        private readonly IDeleteService _deleteService;

        public DeleteController(IDeleteService deleteService)
        {
            _deleteService = deleteService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductAsync(int ProductId)
        {
            var result = await _deleteService.DeleteProductAsync(ProductId);
            if (!result)
            {
                return View("_NotFound");
            }

            return RedirectToAction("Index", "Warehouse");
        }

    }
}