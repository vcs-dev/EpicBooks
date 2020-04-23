using Domain.DadosCliente;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Loja.Controllers
{
    public class ContaController : Controller
    {
        [Area("Loja")]
        public IActionResult Logar()
        {
            return View();
        }

        [Area("Loja")]
        [HttpPost]
        public IActionResult Logar(string usuario, string senha)
        {
            return View();
        }

        [Area("Loja")]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [Area("Loja")]
        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            return View();
        }
    }
}