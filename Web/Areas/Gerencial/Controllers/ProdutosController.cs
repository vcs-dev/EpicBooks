using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Gerencial.Controllers
{
    public class ProdutosController : Controller
    {
        [Area("Gerencial")]
        public IActionResult CadastrarProduto()
        {
            return View();
        }
    }
}