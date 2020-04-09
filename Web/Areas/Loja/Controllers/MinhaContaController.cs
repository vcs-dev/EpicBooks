using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application;
using Core.Impl.Control;
using Domain.Negocio;
using Domain.Produto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Util;

namespace Web.Areas.Loja.Controllers
{
    public class MinhaContaController : Controller
    {
        Result resultado;

        [Area("Loja")]
        public IActionResult Index()
        {
            return View();
        }

        [Area("Loja")]
        public IActionResult MeusPedidos()
        {
            List<Pedido> pedidos;
            resultado = new Facade().Consultar(new Pedido { UsuarioId = HttpContext.Session.Get<int>("idUsuario") });//Busca cupons
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                ViewBag.Mensagem = resultado.Msg;
                return View("meusPedidos", new List<Pedido>());
            }

            pedidos = new List<Pedido>();
            foreach (var item in resultado.Entidades)
            {
                pedidos.Add((Pedido)item);
            }
            return View("meusPedidos", pedidos);
        }

        [Area("Loja")]
        public IActionResult Detalhes(int id)
        {
            List<Pedido> pedidos;
            List<Livro> produtos = new List<Livro>();

            resultado = new Facade().Consultar(new Pedido { Id = id });
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                ViewBag.Mensagem = resultado.Msg;
                return View("detalhes", new Pedido());
            }

            pedidos = new List<Pedido>();
            foreach (var item in resultado.Entidades)
            {
                pedidos.Add((Pedido)item);
            }

            foreach (var item in pedidos.FirstOrDefault().ItensPedido)
            {
                resultado = new Facade().Consultar(new Livro { Id = item.Id });
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    ViewBag.Mensagem = resultado.Msg;
                    return View("detalhes", new Pedido());
                }
                foreach (var livro in resultado.Entidades)
                {
                    produtos.Add((Livro)livro);
                }
            }

            foreach (var item in pedidos.FirstOrDefault().ItensPedido)
            {
                foreach (var prod in produtos)
                {
                    if (prod.Id == item.Id)
                        item.Produto.Nome = prod.Nome;
                }
            }

            return View("detalhes", pedidos.FirstOrDefault());
        }

        [Area("Loja")]
        public IActionResult SolicitarTroca(int pedidoId)
        {
            return View();
        }

        [Area("Loja")]
        [HttpPost]
        public IActionResult EnviarSolicitacaoTroca(int pedidoId)
        {
            return View();
        }
    }
}
