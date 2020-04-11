using System.Collections.Generic;
using System.Linq;
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
        private Result resultado;

        [Area("Loja")]
        public IActionResult Index()
        {
            return View();
        }

        [Area("Loja")]
        public IActionResult MeusPedidos()
        {
            resultado = new Facade().Consultar(new Pedido { UsuarioId = HttpContext.Session.Get<int>("idUsuario") });
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View("meusPedidos", new List<Pedido>());
            }
            if (resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "No momento você não possui pedidos.";
                return View("meusPedidos", new List<Pedido>());
            }
            List<Pedido> pedidos = new List<Pedido>();
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
        public JsonResult EnviarSolicitacaoTroca(int itemId, int qtde, int pedidoId)
        {
            string msg;
            Troca troca = new Troca 
            { 
                ItemId = itemId,
                PedidoId = pedidoId,
                Qtde = qtde,
                Status = 'P'
            };
            resultado = new Facade().Salvar(troca);
            if (resultado.Msg != null)
                msg = resultado.Msg;
            else
                msg = "Troca solicitada com sucesso!";

            return Json("{\"Mensagem\":" + "\"" + msg.Replace("\n", " ") + "\"}");
        }

        [Area("Loja")]
        public IActionResult MinhasTrocas()
        {
            resultado = new Facade().Consultar(new Troca { UsuarioId = HttpContext.Session.Get<int>("idUsuario") });
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View("minhasTrocas", new List<Troca>());
            }
            if(resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "No momento você não possui solicitações de troca.";
                return View("minhasTrocas", new List<Troca>());
            }
            List<Troca> trocas = new List<Troca>();
            foreach (var item in resultado.Entidades)
            {
                trocas.Add((Troca)item);
            }

            return View("minhasTrocas", trocas);
        }
    }
}
