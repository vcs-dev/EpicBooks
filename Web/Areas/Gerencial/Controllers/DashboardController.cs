using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Negocio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Areas.Gerencial.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Gerencial")]
        public IActionResult Index()
        {
            return View();
        }

        [Area("Gerencial")]
        public JsonResult GerarGraficoLinhas(string dataInicial, string dataFinal)
        {
            GraficoLinhas grafico = new GraficoLinhas();
            return Json(JsonConvert.SerializeObject(grafico));
        }

        [Area("Gerencial")]
        public JsonResult GerarGraficoTorta(string dataInicial, string dataFinal)
        {
            return Json("");
        }
    }
}