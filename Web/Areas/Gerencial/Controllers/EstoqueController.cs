using System.Collections.Generic;
using System.Linq;
using Core.Application;
using Core.Impl.Control;
using Domain.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Gerencial.Controllers
{
    public class EstoqueController : Controller
    {
        private Result resultado;

        [Area("Gerencial")]
        public IActionResult Index()
        {
            resultado = new Facade().Consultar(new Estoque());
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View(new List<Estoque>());
            }
            if (resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "Ainda não há produtos em estoque.";
                return View(new List<Estoque>());
            }
            List<Estoque> estoqueProds = new List<Estoque>();
            foreach (var item in resultado.Entidades)
            {
                estoqueProds.Add((Estoque)item);
            }
            return View(estoqueProds);
        }

        [Area("Gerencial")]
        public PartialViewResult Consultar(string stringBusca)
        {
            if (string.IsNullOrEmpty(stringBusca) || string.IsNullOrWhiteSpace(stringBusca))
                return PartialView("_estoqueProds");

            resultado = new Facade().Consultar(new Estoque { NomeProduto = stringBusca });
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return PartialView("_estoqueProds");
            }
            if (resultado.Entidades.Count == 0)
                return PartialView("_estoqueProds", new List<Estoque>());
            List<Estoque> estoqueProds = new List<Estoque>();
            foreach (var item in resultado.Entidades)
            {
                estoqueProds.Add((Estoque)item);
            }

            return PartialView("_estoqueProds", estoqueProds);
        }

        [Area("Gerencial")]
        public IActionResult DarEntrada(int id)
        {
            if (id <= 0)
            {
                ViewBag.Mensagem = "Id de produto inválido.";
                return PartialView("_estoqueProds");
            }
            resultado = new Facade().Consultar(new Estoque { ProdutoId = id });
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return PartialView("_estoqueProds");
            }
            if (resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "Produto não econtrado.";
                return PartialView("_estoqueProds", new List<Estoque>());
            }
            List<Estoque> estoqueProds = new List<Estoque>();
            foreach (var item in resultado.Entidades)
            {
                estoqueProds.Add((Estoque)item);
            }
            //teste
            EntradaEstoque entradaEstoque = new EntradaEstoque
            {
                ProdutoId = id,
                NomeProduto = estoqueProds.FirstOrDefault().NomeProduto
            };

            return View("darEntrada", entradaEstoque);
        }

        [Area("Gerencial")]
        [HttpPost]
        public IActionResult DarEntrada(EntradaEstoque entradaEstoque)
        {
            resultado = new Facade().Salvar(entradaEstoque);
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View("darEntrada", entradaEstoque);
            }
            ViewBag.Mensagem = "Entrada de estoque registrada com sucesso.";
            return View("darEntrada", entradaEstoque);
        }
    }
}