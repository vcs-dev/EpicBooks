using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application;
using Core.Impl.Business;
using Core.Impl.Control;
using Domain.Negocio;
using Domain.Produto;
using Microsoft.AspNetCore.Mvc;
using Web.Util;

namespace Web.Areas.Loja.Controllers
{
    public class CarrinhoController : Controller
    {
        private Result resultado;

        [Area("Loja")]
        public IActionResult Index()
        {
            Carrinho carrinho = new Carrinho();
            ItemPedido itemPedido;
            if (HttpContext.Session.Keys.Count() > 0)
            {
                foreach (var item in HttpContext.Session.Keys)
                {
                    if (int.TryParse(item, out int num))
                    {//Se for uma chave de int entra na estrutura abaixo
                     //Produtos sao inseridos na Session com chave int
                        itemPedido = SessionHelper.Get<ItemPedido>(HttpContext.Session, item);
                        carrinho.ItensPedido.Add(itemPedido);
                    }
                }
            }
            foreach (var item in carrinho.ItensPedido)
            {
                carrinho.QtdeTotalItens += item.Qtde;
            }
            if (!string.IsNullOrEmpty(carrinho.Cep) && !string.IsNullOrWhiteSpace(carrinho.Cep))
                carrinho.Frete =  CalculoFrete.Calcular(carrinho.Cep, carrinho.ItensPedido.Count());

            return View(carrinho);
        }

        [Area("Loja")]
        public IActionResult AdicionarItem(int id)
        {
            List<Livro> livros;
            ItemPedido itemPedido = new ItemPedido();
            Livro livro = new Livro
            {
                Id = id
            };

            resultado = new Facade().Consultar(livro);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return PartialView("_produtos");
            }
            else
            {
                livros = new List<Livro>();
                foreach (var item in resultado.Entidades)
                {
                    livros.Add((Livro)item);
                }
                itemPedido.Qtde = 1;
                itemPedido.HoraInclusao = DateTime.Now;
                itemPedido.Produto = livros.FirstOrDefault();
                SessionHelper.Set<ItemPedido>(HttpContext.Session, itemPedido.Produto.Id.ToString(), itemPedido);
                return RedirectToAction("index");
            }
        }

        [Area("Loja")]
        public PartialViewResult AumentarQtde(int id)
        {
            ItemPedido itemPedido;
            List<ItemPedido> itensPedido = new List<ItemPedido>();

            itemPedido = SessionHelper.Get<ItemPedido>(HttpContext.Session, id.ToString());
            itemPedido.Qtde += 1;
            SessionHelper.Set<ItemPedido>(HttpContext.Session, itemPedido.Produto.Id.ToString(), itemPedido);

            if (HttpContext.Session.Keys.Count() > 0)
            {
                foreach (var item in HttpContext.Session.Keys)
                {
                    if (int.TryParse(item, out int num))
                    {//Se for uma chave de int entra na estrutura abaixo
                     //Produtos sao inseridos na Session com chave int
                        itemPedido = SessionHelper.Get<ItemPedido>(HttpContext.Session, item);
                        itensPedido.Add(itemPedido);
                    }
                }
            }
            return PartialView("_itensPedido", itensPedido);
        }

        [Area("Loja")]
        public PartialViewResult DiminuirQtde(int id)
        {
            ItemPedido itemPedido;
            List<ItemPedido> itensPedido = new List<ItemPedido>();

            itemPedido = SessionHelper.Get<ItemPedido>(HttpContext.Session, id.ToString());
            if (itemPedido.Qtde > 1)
                itemPedido.Qtde -= 1;
            SessionHelper.Set<ItemPedido>(HttpContext.Session, itemPedido.Produto.Id.ToString(), itemPedido);

            if (HttpContext.Session.Keys.Count() > 0)
            {
                foreach (var item in HttpContext.Session.Keys)
                {
                    if (int.TryParse(item, out int num))
                    {//Se for uma chave de int entra na estrutura abaixo
                     //Produtos sao inseridos na Session com chave int
                        itemPedido = SessionHelper.Get<ItemPedido>(HttpContext.Session, item);
                        itensPedido.Add(itemPedido);
                    }
                }
            }
            return PartialView("_itensPedido", itensPedido);
        }

        [Area("Loja")]
        public PartialViewResult RemoverItem(int id)
        {
            ItemPedido itemPedido;
            List<ItemPedido> itensPedido = new List<ItemPedido>();

            itemPedido = SessionHelper.Get<ItemPedido>(HttpContext.Session, id.ToString());
            if (itemPedido != null)
                HttpContext.Session.Remove(id.ToString());
            if (HttpContext.Session.Keys.Count() > 0)
            {
                foreach (var item in HttpContext.Session.Keys)
                {
                    if (int.TryParse(item, out int num))
                    {//Se for uma chave de int entra na estrutura abaixo
                     //Produtos sao inseridos na Session com chave int
                        itemPedido = SessionHelper.Get<ItemPedido>(HttpContext.Session, item);
                        itensPedido.Add(itemPedido);
                    }
                }
            }
            return PartialView("_itensPedido", itensPedido);
        }

        [Area("Loja")]
        public JsonResult CalcularFrete(string cep, int qtdeItens)
        {
            return Json(CalculoFrete.Calcular(cep, qtdeItens));
        }
        //[Area("Loja")]
        //public PartialViewResult AdicionarCupomPromo()
        //{
        //}

        //[Area("Loja")]
        //public PartialViewResult AdicionarCupomTroca()
        //{
        //}
        //[Area("Loja")]
        //public PartialViewResult AdicionarCartaoCredito(int idCartao, double valor)
        //{
        //}
    }
}