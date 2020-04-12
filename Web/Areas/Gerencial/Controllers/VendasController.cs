using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application;
using Core.Impl.Control;
using Domain.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Gerencial.Controllers
{
    public class VendasController : Controller
    {
        private Result resultado;

        [Area("Gerencial")]
        public IActionResult Index()
        {
            resultado = new Facade().Consultar(new Pedido());
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View();
            }
            if (resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "Ainda não existem vendas no sistema.";
                return View(new List<Pedido>());
            }
            IList<Pedido> pedidos = new List<Pedido>();
            foreach (var item in resultado.Entidades)
            {
                pedidos.Add((Pedido)item);
            }
            pedidos = pedidos.Where(x => x.Status != 'P' && x.Status != 'R').ToList();

            return View(pedidos);
        }
    }
}