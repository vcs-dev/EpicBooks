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
    public class TrocasController : Controller
    {
        private Result resultado;

        [Area("Gerencial")]
        public IActionResult Index()
        {
            resultado = new Facade().Consultar(new Troca { Status = 'P' });
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
    }
}