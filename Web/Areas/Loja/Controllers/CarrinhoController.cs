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

            if (HttpContext.Session.Keys.Count() > 0)
                carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");//Se o carrinho existir, carrega
            else
                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);//Senao, cria

            foreach (var item in carrinho.ItensPedido)
            {
                carrinho.QtdeTotalItens += item.Qtde;
            }
            if (!string.IsNullOrEmpty(carrinho.Cep) && !string.IsNullOrWhiteSpace(carrinho.Cep))
                carrinho.Frete = CalculoFrete.Calcular(carrinho.Cep, carrinho.ItensPedido.Count());

            return View(carrinho);
        }

        [Area("Loja")]
        public IActionResult AdicionarItem(int id)
        {
            List<Livro> livros;
            Carrinho carrinho;
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

                if (HttpContext.Session.Keys.Count() > 0)
                    carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");//Se o carrinho existir, carrega
                else
                    SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", new Carrinho());//Senao, cria

                carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");

                ItemPedido itemPed = carrinho.ItensPedido.Find(x => x.Produto.Id == id);//Verifica se o produto ja esta no carrinho
                if (itemPed != null)
                    carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde += 1;//Se sim, incrementa qtde em 1
                else
                    carrinho.ItensPedido.Add(itemPedido);//Senao, adiciona o novo item ao carrinho

                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);
                return RedirectToAction("index");
            }
        }

        [Area("Loja")]
        public PartialViewResult AumentarQtde(int id)
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");

            ItemPedido itemPed = carrinho.ItensPedido.Find(x => x.Produto.Id == id);//Verifica se o produto ja esta no carrinho
                carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde += 1;//Se sim, incrementa qtde em 1

            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

            return PartialView("_itensPedido", carrinho.ItensPedido);
        }

        [Area("Loja")]
        public PartialViewResult DiminuirQtde(int id)
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");

            ItemPedido itemPed = carrinho.ItensPedido.Find(x => x.Produto.Id == id);//Verifica se o produto ja esta no carrinho
            if(carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde > 1)
                carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde -= 1;//Decrementa qtde em 1 se a qtde for maior que 1

            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

            return PartialView("_itensPedido", carrinho.ItensPedido);
        }

        [Area("Loja")]
        public PartialViewResult RemoverItem(int id)
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");

            ItemPedido itemPed = carrinho.ItensPedido.Find(x => x.Produto.Id == id);//Verifica se o produto ja esta no carrinho
            if (itemPed != null)
                carrinho.ItensPedido.Remove(itemPed);//Decrementa qtde em 1 se a qtde for maior que 1

            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

            return PartialView("_itensPedido", carrinho.ItensPedido);
        }

        [Area("Loja")]
        public JsonResult CalcularFrete(string cep)
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            return Json("{valor : " + CalculoFrete.Calcular(cep, carrinho.QtdeTotalItens) + "}");
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