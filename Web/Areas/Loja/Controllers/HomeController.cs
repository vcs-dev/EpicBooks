﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Application;
using Core.Impl.Control;
using Domain.Produto;
using Microsoft.AspNetCore.Mvc;

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
                foreach (var item in livros)
                {
                    if (item.Status == 1)
                        livrosAtivos.Add(item);
                }
                var nome = "Fulano da Silva";
                var id = "1";
                HttpContext.Session.Set("nomeUsuario", Encoding.ASCII.GetBytes(nome));
                HttpContext.Session.Set("idUsuario", Encoding.ASCII.GetBytes(id));

                return View(livrosAtivos);
            }
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