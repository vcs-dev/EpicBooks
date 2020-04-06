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
            Cupom cupom = new Cupom();
            List<Cupom> cupons;

            if (HttpContext.Session.Keys.Count() > 0)
                carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");//Se o carrinho existir, carrega
            else
                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);//Senao, cria

            carrinho.QtdeTotalItens = 0;
            foreach (var item in carrinho.ItensPedido)
            {
                carrinho.QtdeTotalItens += item.Qtde;
            }
            if (!string.IsNullOrEmpty(carrinho.Cep) && !string.IsNullOrWhiteSpace(carrinho.Cep))
                carrinho.ValorFrete = CalculoFrete.Calcular(carrinho.Cep, carrinho.ItensPedido.Count());
            if (string.IsNullOrEmpty(carrinho.Cep))
                carrinho.Cep = "";

            cupom.UsuarioId = 1;
            cupom.DataExpiracao = DateTime.Now;
            cupom.Usado = 0;
            resultado = new Facade().Consultar(cupom);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return PartialView("_resumo", carrinho);
            }
            else
            {
                cupons = new List<Cupom>();
                foreach (var item in resultado.Entidades)
                {
                    cupons.Add((Cupom)item);
                }
            }
            ViewBag.CuponsTroca = cupons;
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

                carrinho.QtdeTotalItens += 1;
                carrinho.HoraUltimaInclusao = DateTime.Now;
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
            carrinho.QtdeTotalItens += 1;

            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

            return PartialView("_itensPedido", carrinho.ItensPedido);
        }

        [Area("Loja")]
        public PartialViewResult DiminuirQtde(int id)
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");

            ItemPedido itemPed = carrinho.ItensPedido.Find(x => x.Produto.Id == id);//Verifica se o produto ja esta no carrinho
            if (carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde > 1)
            {
                carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde -= 1;//Decrementa qtde em 1 se a qtde for maior que 1
                carrinho.QtdeTotalItens -= 1;
                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);
            }

            return PartialView("_itensPedido", carrinho.ItensPedido);
        }

        [Area("Loja")]
        public PartialViewResult RemoverItem(int id)
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");

            ItemPedido itemPed = carrinho.ItensPedido.Find(x => x.Produto.Id == id);//Verifica se o produto ja esta no carrinho
            if (itemPed != null)
            {
                carrinho.QtdeTotalItens -= itemPed.Qtde;
                carrinho.ItensPedido.Remove(itemPed);//Decrementa qtde em 1 se a qtde for maior que 1
                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);
            }
            if(carrinho.ItensPedido.Count() > 0)
                return PartialView("_itensPedido", carrinho.ItensPedido);
            else
                return PartialView(null);
        }

        //[Area("Loja")]
        //public JsonResult CalcularFrete(string cep)
        //{
        //    Carrinho carrinho;

        //    carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
        //    carrinho.ValorFrete = CalculoFrete.Calcular(cep, carrinho.QtdeTotalItens);
        //    SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

        //    return Json("{\"valor\":" + "\"" + carrinho.ValorFrete + "\"}");
        //}

        [Area("Loja")]
        public PartialViewResult CalcularFrete(string cep)
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            carrinho.ValorFrete = CalculoFrete.Calcular(cep, carrinho.QtdeTotalItens);
            carrinho.Cep = cep;
            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

            return PartialView("_resumo", carrinho);
        }

        [Area("Loja")]
        public PartialViewResult AdicionarCupomTroca(string codCupom)
        {
            Carrinho carrinho;
            Cupom cupom;
            List<Cupom> cupons = new List<Cupom>();

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            cupom = carrinho.CuponsTroca.Find(x => x.Codigo == codCupom);//Verifica se o cupom ja foi adicionado
            if (cupom == null)
            {
                cupom = new Cupom();
                cupom.Codigo = codCupom;
                cupom.DataExpiracao = DateTime.Now;
                cupom.Usado = 0;
                resultado = new Facade().Consultar(cupom);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    TempData["MsgErro"] = resultado.Msg;
                    return PartialView("_resumo", carrinho);
                }
                else
                {
                    cupons = new List<Cupom>();
                    foreach (var item in resultado.Entidades)
                    {
                        cupons.Add((Cupom)item);
                    }
                    cupom = cupons.FirstOrDefault();
                }
                carrinho.CuponsTroca.Add(cupom);//Se nao foi, adiciona
                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);
            }
            return PartialView("_resumo", carrinho);
        }

        [Area("Loja")]
        public PartialViewResult RemoverCupomTroca(string codCupom)
        {
            Carrinho carrinho;
            Cupom cupom;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            cupom = carrinho.CuponsTroca.Find(x => x.Codigo == codCupom);//Verifica se o cupom ja foi adicionado
            if (cupom != null)
                carrinho.CuponsTroca.Remove(cupom);
            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

            return PartialView("_resumo", carrinho);
        }

        [Area("Loja")]
        public PartialViewResult AdicionarCupomPromo(string codCupom)
        {
            Carrinho carrinho;
            Cupom cupom = new Cupom();
            List<Cupom> cupons;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            if (!string.IsNullOrEmpty(carrinho.CupomPromocional.Codigo) || 
                carrinho.CupomPromocional.Codigo != codCupom)
            {
                cupom.Codigo = codCupom;
                cupom.DataExpiracao = DateTime.Now;
                cupom.Usado = null;
                resultado = new Facade().Consultar(cupom);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    TempData["MsgErro"] = resultado.Msg;
                    return PartialView(null);
                }
                else
                {
                    cupons = new List<Cupom>();
                    foreach (var item in resultado.Entidades)
                    {
                        cupons.Add((Cupom)item);
                    }
                    cupom = cupons.FirstOrDefault();
                }
                carrinho.CupomPromocional = cupom;
                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);
            }

            return PartialView("_resumo", carrinho);
        }

        [Area("Loja")]
        public PartialViewResult RemoverCupomPromo()
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            carrinho.CupomPromocional = new Cupom();
            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

            return PartialView("_resumo", carrinho);
        }


        [Area("Loja")]
        public PartialViewResult AtualizarResumo()
        {
            Carrinho carrinho;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            return PartialView("_resumo", carrinho);
        }

        [Area("Loja")]
        [HttpPost]
        public IActionResult EnviarPedido(Carrinho carrinho)
        {
            Pedido pedido = new Pedido();
            resultado = new Facade().Salvar(pedido);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return View(carrinho);
            }
            TempData["MsgSucesso"] = "Pedido " + resultado.Entidades.FirstOrDefault().Id + " realizado com sucesso.";
            return View(carrinho);
        }
    }
}