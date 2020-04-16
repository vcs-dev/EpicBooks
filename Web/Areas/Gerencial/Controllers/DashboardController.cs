using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Gerencial.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Gerencial")]
        public IActionResult Index()
        {
            return View();
        }
    }
}