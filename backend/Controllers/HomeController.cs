using Microsoft.AspNetCore.Mvc;
using MiodOdStaniula.Models;
using System.Diagnostics;

namespace MiodOdStaniula.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}