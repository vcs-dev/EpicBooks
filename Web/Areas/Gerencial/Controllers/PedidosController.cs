using Core.Application;
using Core.Impl.Control;
using Domain.Negocio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Web.Areas.Gerencial.Controllers
{
    public class PedidosController : Controller
    {
        private Result resultado;
        [Area("Gerencial")]
        public IActionResult Index()
        {
            resultado = new Facade().Consultar(new Pedido());
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View(new List<Pedido>());
            }
            if (resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "Não existem pedidos.";
                return View(new List<Pedido>());
            }
            List<Pedido> trocas = new List<Pedido>();
            foreach (var item in resultado.Entidades)
            {
                trocas.Add((Pedido)item);
            }

            return View(trocas);
        }

        [Area("Gerencial")]
        public JsonResult AlterarStatus(int pedidoId, char status)
        {
            string msg;
            resultado = new Facade().Alterar(new Pedido
            {
                Id = pedidoId,
                Status = status,
                IsAlteracaoGerencial = true
            });
            if (resultado.Msg != null)
                msg = resultado.Msg;
            else
                msg = "Status alterado com sucesso!";
            return Json("{\"Mensagem\":" + "\"" + msg.Replace("\n", " ") + "\"}");
        }
    }
}