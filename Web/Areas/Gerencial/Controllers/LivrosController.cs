using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Produto;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Gerencial.Controllers
{
    public class LivrosController : Controller
    {
        [Area("Gerencial")]
        public IActionResult Cadastrar()
        {
            return View(new Livro());
        }

        [Area("Gerencial")]
        [HttpPost]
        public IActionResult Cadastrar(Livro livro)
        {
            return View();
        }
    }
}