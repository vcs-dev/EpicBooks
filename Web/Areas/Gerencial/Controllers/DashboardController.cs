using System;
using System.Collections.Generic;
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
            GraficoLinhas grafico = new GraficoLinhas();
            DataSetGraficoLinhas dataSet = new DataSetGraficoLinhas();

            resultado = new Facade().Consultar(new Faturamento { DataInicial = dataInicial, DataFinal = dataFinal });
            if (resultado.Msg != null)
            {
                grafico.mensagemErro = resultado.Msg;
                return Json(JsonConvert.SerializeObject(grafico));
            }
            grafico.labels = DataHelper.RetornaPeriodo(dataInicial, dataFinal);
            if (resultado.Entidades.Count == 0)
            {
                grafico.mensagemErro = "Sem dados no período informado.";
                return Json(JsonConvert.SerializeObject(grafico));
            }
            List<Faturamento> faturamentos = new List<Faturamento>();
            foreach (var item in resultado.Entidades)
            {
                faturamentos.Add((Faturamento)item);
            }
            dataSet.label = "Faturamento";
            foreach (var item in grafico.labels)
            {
                dataSet.data.Add(0);
            }
            grafico.datasets.Add(dataSet);
            Faturamento faturamento;
            foreach (var item in grafico.labels)
            {
                faturamento = faturamentos.Find(x => x.Data.Equals(item));
                if(faturamento != null)
                    grafico.datasets[0].data[grafico.labels.IndexOf(item)] = Convert.ToDouble(faturamento.Valor.ToString("######0.00"));
            }
            //lucro
            dataSet = new DataSetGraficoLinhas();
            resultado = new Facade().Consultar(new Lucro { DataInicial = dataInicial, DataFinal = dataFinal });
            if (resultado.Msg != null)
            {
                grafico.mensagemErro = resultado.Msg;
                return Json(JsonConvert.SerializeObject(grafico));
            }
            //grafico.labels = DataHelper.RetornaPeriodo(dataInicial, dataFinal);
            if (resultado.Entidades.Count == 0)
            {
                grafico.mensagemErro = "Sem dados no período informado.";
                return Json(JsonConvert.SerializeObject(grafico));
            }
            List<Lucro> lucros = new List<Lucro>();
            foreach (var item in resultado.Entidades)
            {
                lucros.Add((Lucro)item);
            }
            dataSet.label = "Lucro";
            foreach (var item in grafico.labels)
            {
                dataSet.data.Add(0);
            }
            grafico.datasets.Add(dataSet);
            Lucro lucro;
            foreach (var item in grafico.labels)
            {
                lucro = lucros.Find(x => x.Data.Equals(item));
                if (lucro != null)
                    grafico.datasets[1].data[grafico.labels.IndexOf(item)] = Convert.ToDouble(lucro.Valor.ToString("######0.00"));
            }
            return Json(JsonConvert.SerializeObject(grafico));
        }

        [Area("Gerencial")]
        public JsonResult GerarGraficoTorta(string dataInicial, string dataFinal)
        {
            GraficoTorta grafico = new GraficoTorta();

            resultado = new Facade().Consultar(new Venda { DataInicial = dataInicial, DataFinal = dataFinal });
            if (resultado.Msg != null)
            {
                grafico.mensagemErro = resultado.Msg;
                return Json(JsonConvert.SerializeObject(grafico));
            }
            if (resultado.Entidades.Count == 0)
            {
                grafico.mensagemErro = "Sem dados no período informado.";
                return Json(JsonConvert.SerializeObject(new GraficoTorta()));
            }
            List<Venda> vendas = new List<Venda>();
            foreach (var item in resultado.Entidades)
            {
                vendas.Add((Venda)item);
            }
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