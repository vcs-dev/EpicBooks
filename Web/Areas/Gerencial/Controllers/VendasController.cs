using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<Pedido> pedidos;
            Pedido pedido = new Pedido
            {
                Status = 'A'
            };

            resultado = new Facade().Consultar(pedido);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return View();
            }
            else
            {
                TempData["MsgSucesso"] = resultado.Msg;
                pedidos = new List<Pedido>();
                foreach (var item in resultado.Entidades)
                {
                    pedidos.Add((Pedido)item);
                }
                return View(pedidos);
            }
        }
    }
}