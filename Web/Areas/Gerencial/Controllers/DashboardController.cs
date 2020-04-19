using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application;
using Core.Impl.Control;
using Domain.Negocio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Util;

namespace Web.Areas.Gerencial.Controllers
{
    public class DashboardController : Controller
    {
        private Result resultado;

        [Area("Gerencial")]
        public IActionResult Index()
        {
            return View();
        }

        [Area("Gerencial")]
        public JsonResult GerarGraficoLinhas(string dataInicial, string dataFinal)
        {
            //GraficoLinhas grafico = new GraficoLinhas();
            //ConjuntoDados dataSet;
            //grafico.labels = DataHelper.RetornaPeriodo(dataInicial, dataFinal);
            //foreach (var item in grafico.labels)
            //{
            //    resultado = new Facade().Consultar(new Venda
            //    {
            //        DataInicial = Convert.ToDateTime("01/" + item),
            //        DataFinal = Convert.ToDateTime("01/" + item).AddDays(30)
            //    });
            //    if (resultado.Msg != null)
            //    {
            //        ViewBag.Mensagem = resultado.Msg;
            //    }
            //    if (resultado.Entidades.Count == 0)
            //    {
            //        dataSet = new ConjuntoDados();
            //        dataSet.data.Add(0);
            //        grafico.datasets.Add(dataSet);
            //    }
            //    else
            //    {
            //        List<Venda> vendas = new List<Venda>();
            //        foreach (var venda in resultado.Entidades)
            //        {
            //            vendas.Add((Venda)venda);
            //        }
            //        dataSet = new ConjuntoDados();
            //        foreach (var dataset in grafico.datasets)
            //        {
            //            foreach (var v in vendas)
            //            {
            //                if (dataset.label == null || v.Data.Equals(item))
            //                {
            //                    dataset.label = v.NomeProduto;
            //                    dataset.data.Add(v.Qtde);
            //                    vendas.Remove(v);
            //                    break;
            //                }

            //            }
            //        }
            //    }
            //}
            ////resultado = new Facade().Consultar(new Venda { DataInicial = dataInicial, DataFinal = dataFinal });
            //GraficoLinhas graficoFinal = new GraficoLinhas();
            //foreach (var item in grafico.datasets)
            //{
            //    if (item.label != null)
            //        graficoFinal.datasets.Add(item);
            //}
            return Json(""/*JsonConvert.SerializeObject(graficoFinal)*/);
        }

        [Area("Gerencial")]
        public JsonResult GerarGraficoTorta(string dataInicial, string dataFinal)
        {
            resultado = new Facade().Consultar(
                new Venda 
                { 
                    DataInicial = Convert.ToDateTime(dataInicial),
                    DataFinal = Convert.ToDateTime(dataFinal),
                    TipoGrafico = "TORTA" 
                }
            );
            if(resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
            }
            if (resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "Sem dados no período.";
            }
            List<Venda> vendas = new List<Venda>();
            foreach (var item in resultado.Entidades)
            {
                vendas.Add((Venda)item);
            }
            GraficoTorta grafico = new GraficoTorta();
            foreach (var item in vendas)
            {
                grafico.labels.Add(item.NomeProduto);
            }
            List<int> data = new List<int>();
            foreach (var item in vendas)
            {
                data.Add(item.Qtde);
            }
            List<string> cores = new List<string>();
            Random random = new Random();
            foreach (var item in grafico.labels)
            {
                cores.Add(string.Format("rgba({0}, {1}, {2}, 0.9)", random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
            }
            DataSetGraficoTorta dataSet = new DataSetGraficoTorta();
            dataSet.backgroundColor = cores;
            dataSet.data = data;
            grafico.datasets.Add(dataSet);
            return Json(JsonConvert.SerializeObject(grafico));
        }
    }
}