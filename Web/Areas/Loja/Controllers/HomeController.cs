using System.Collections.Generic;
using System.Linq;
using Core.Application;
using Core.Impl.Control;
using Domain.Produto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Util;

namespace Web.Areas.Loja.Controllers
{
    public class HomeController : Controller
    {
        private Result resultado;

        [Area("Loja")]
        public IActionResult Index()
        {
            List<Livro> livros, livrosAtivos;
            Livro livro = new Livro();

            livrosAtivos = new List<Livro>();

            resultado = new Facade().Consultar(livro);
            if (resultado.Msg != null)
                TempData["MsgErro"] = resultado.Msg;
            else
            {
                TempData["MsgSucesso"] = resultado.Msg;
                livros = new List<Livro>();
                foreach (var item in resultado.Entidades)
                {
                    livros.Add((Livro)item);
                }
                foreach (var item in livros)
                {
                    if (item.Status == 1)
                        livrosAtivos.Add(item);
                }
                if (HttpContext.Session.GetString("nomeUsuario") != string.Empty)
                    ViewBag.NomeUsuario = HttpContext.Session.GetString("nomeUsuario");
            }
            return View(livrosAtivos);
        }

        [Area("Loja")]
        public IActionResult Produto(int id)
        {
            List<Livro> livros, livrosAtivos;
            Livro livro = new Livro();

            livrosAtivos = new List<Livro>();
            livro.Id = id;

            resultado = new Facade().Consultar(livro);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return View();
            }
            else
            {
                TempData["MsgSucesso"] = resultado.Msg;
                livros = new List<Livro>();

                foreach (var item in resultado.Entidades)
                {
                    livros.Add((Livro)item);
                }
                if (livros.FirstOrDefault().Status == 1)
                    livrosAtivos.Add(livros.FirstOrDefault());
                return View(livrosAtivos.FirstOrDefault());
            }
        }
    }
}