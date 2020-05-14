using Core.Application;
using Core.Impl.Control;
using Domain.DadosCliente;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Loja.Controllers
{
    public class ContaController : Controller
    {
        private Result resultado;

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
            resultado = new Facade().Salvar(usuario);
            if(resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View(usuario);
            }
            ViewBag.Mensagem = "Cadastro efetuado com sucesso!";
            return View();
        }
    }
}