using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Loja.Controllers
{
    public class HomeController : Controller
    {
        [Area("Loja")]
        public IActionResult Index()
        {
            return View();
        }
    }
}