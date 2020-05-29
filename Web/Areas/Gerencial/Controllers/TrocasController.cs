using System;
using System.Collections.Generic;
using Core.Application;
using Core.Impl.Control;
using Domain.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Gerencial.Controllers
{
    public class TrocasController : Controller
    {
        private Result resultado;

        [Area("Gerencial")]
        public IActionResult Index()
        {
            resultado = new Facade().Consultar(new Troca());
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View(new List<Troca>());
            }
            if (resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "Não existem solicitações de troca pendentes.";
                return View(new List<Troca>());
            }
            List<Troca> trocas = new List<Troca>();
            foreach (var item in resultado.Entidades)
            {
                trocas.Add((Troca)item);
            }

            return View(trocas);
        }
        [Area("Gerencial")]
        public JsonResult ConfirmarRecebimentoItem(int pedidoId, int itemId, int usuarioId, int qtde, byte voltaParaEstoque)
        {
            string msg;
            resultado = new Facade().Alterar(
                new Troca
                {
                    UsuarioId = usuarioId,
                    PedidoId = pedidoId,
                    ItemId = itemId,
                    Qtde = qtde,
                    Status = 'C',
                    EstoqueObservacao = "REENTRADA - TROCA",
                    VoltaParEstoque = Convert.ToBoolean(voltaParaEstoque),
                });
            if (resultado.Msg != null)
                msg = resultado.Msg;
            else
                msg = "Confirmação de recebimento salva com sucesso!";
            return Json("{\"Mensagem\":" + "\"" + msg.Replace("\n", " ") + "\"}");
        }

        [Area("Gerencial")]
        public JsonResult AutorizarTroca(int pedidoId, int itemId)
        {
            string msg;
            resultado = new Facade().Alterar(new Troca
            {
                PedidoId = pedidoId,
                ItemId = itemId,
                Status = 'A'
            });
            if (resultado.Msg != null)
                msg = resultado.Msg;
            else
                msg = "Troca autorizada com sucesso!";
            return Json("{\"Mensagem\":" + "\"" + msg.Replace("\n", " ") + "\"}");
        }

        [Area("Gerencial")]
        public JsonResult NegarTroca(int pedidoId, int itemId)
        {
            string msg;
            resultado = new Facade().Alterar(new Troca
            {
                PedidoId = pedidoId,
                ItemId = itemId,
                Status = 'N'
            });
            if (resultado.Msg != null)
                msg = resultado.Msg;
            else
                msg = "Troca negada com sucesso!";
            return Json("{\"Mensagem\":" + "\"" + msg.Replace("\n", " ") + "\"}");
        }
    }
}