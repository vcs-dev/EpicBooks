using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Application;
using Core.Impl.Business;
using Core.Impl.Control;
using Domain.DadosCliente;
using Domain.Negocio;
using Domain.Produto;
using Microsoft.AspNetCore.Http;
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
            Carrinho carrinho;
            Cupom cupom = new Cupom();
            Endereco endereco = new Endereco();
            CartaoDeCredito cartao = new CartaoDeCredito();
            List<Cupom> cupons;
            List<Endereco> enderecos;
            List<CartaoDeCredito> cartoes;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            if (carrinho == null)
                carrinho = new Carrinho();
            else
            {
                char statusPedido = HttpContext.Session.Get<char>("statusPedido");
                if (statusPedido != '\0' && statusPedido == 'A')
                {
                    carrinho = new Carrinho();
                    HttpContext.Session.Remove("statusPedido");
                }
            }

            carrinho.QtdeTotalItens = 0;
            foreach (var item in carrinho.ItensPedido)
            {
                carrinho.QtdeTotalItens += item.Qtde;
            }
            if (!string.IsNullOrEmpty(carrinho.Cep) && !string.IsNullOrWhiteSpace(carrinho.Cep))
                carrinho.ValorFrete = CalculoFrete.Calcular(carrinho.Cep, carrinho.ItensPedido.Count());
            if (string.IsNullOrEmpty(carrinho.Cep))
                carrinho.Cep = "";

            cupom.UsuarioId = HttpContext.Session.Get<int>("idUsuario");
            cupom.Tipo = 'T';
            cupom.Usado = 0;

            resultado = new Facade().Consultar(cupom);//Busca cupons
            if (!string.IsNullOrEmpty(resultado.Msg))
                ViewBag.Mensagem = resultado.Msg;
            else
            {
                cupons = new List<Cupom>();
                foreach (var item in resultado.Entidades)
                {
                    cupons.Add((Cupom)item);
                }
                ViewBag.CuponsTroca = cupons;
            }

            endereco.UsuarioId = HttpContext.Session.Get<int>("idUsuario");

            resultado = new Facade().Consultar(endereco);//Busca enderecos
            if (!string.IsNullOrEmpty(resultado.Msg))
                ViewBag.Mensagem = resultado.Msg;
            else
            {
                enderecos = new List<Endereco>();
                foreach (var item in resultado.Entidades)
                {
                    enderecos.Add((Endereco)item);
                }
                ViewBag.Enderecos = enderecos;
            }

            cartao.UsuarioId = HttpContext.Session.Get<int>("idUsuario");

            resultado = new Facade().Consultar(cartao);//Busca cartoes
            if (!string.IsNullOrEmpty(resultado.Msg))
                ViewBag.Mensagem = resultado.Msg;
            else
            {
                cartoes = new List<CartaoDeCredito>();
                foreach (var item in resultado.Entidades)
                {
                    cartoes.Add((CartaoDeCredito)item);
                }
                ViewBag.Cartoes = cartoes;
            }
            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);
            string msg = HttpContext.Session.GetString("mensagemCarrinho");

            if (!string.IsNullOrEmpty(msg) && !string.IsNullOrWhiteSpace(msg))
            {
                ViewBag.Mensagem = msg;
                HttpContext.Session.Remove("mensagemCarrinho");
            }
                
            return View(carrinho);
        }

        [Area("Loja")]
        public IActionResult AdicionarItem(int id)
        {
            List<Livro> livros;
            List<Estoque> estoqueProds;
            Carrinho carrinho;
            Estoque estoque;
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

                carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
                if (carrinho == null)
                {
                    carrinho = new Carrinho();
                    HttpContext.Session.Set("statusPedido", '\0');
                }

                ItemPedido itemPed = carrinho.ItensPedido.Find(x => x.Produto.Id == id);//Verifica se o produto ja esta no carrinho

                if (itemPed != null)
                {
                    estoque = new Estoque
                    {
                        ProdutoId = id
                    };
                    resultado = new Facade().Consultar(estoque);//Consulta estoque
                    if (!string.IsNullOrEmpty(resultado.Msg))
                    {
                        TempData["MsgErro"] = resultado.Msg;
                        return PartialView("_produtos");
                    }
                    else
                    {
                        estoqueProds = new List<Estoque>();
                        foreach (var item in resultado.Entidades)
                        {
                            estoqueProds.Add((Estoque)item);
                        }
                        if (estoqueProds.FirstOrDefault().Qtde >= carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde + 1)
                        {
                            carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde += 1;//Se sim, incrementa qtde em 1
                            carrinho.QtdeTotalItens += 1;
                            carrinho.HoraUltimaInclusao = DateTime.Now;
                        }
                    }
                }
                else
                {
                    estoque = new Estoque
                    {
                        ProdutoId = id
                    };
                    resultado = new Facade().Consultar(estoque);//Consulta estoque
                    if (!string.IsNullOrEmpty(resultado.Msg))
                    {
                        TempData["MsgErro"] = resultado.Msg;
                        return PartialView("_produtos");
                    }
                    else
                    {
                        estoqueProds = new List<Estoque>();
                        foreach (var item in resultado.Entidades)
                        {
                            estoqueProds.Add((Estoque)item);
                        }
                        if (estoqueProds.FirstOrDefault().Qtde >= 1)
                        {
                            carrinho.ItensPedido.Add(itemPedido);
                            carrinho.QtdeTotalItens += 1;
                            carrinho.HoraUltimaInclusao = DateTime.Now;
                        }
                    }
                }
                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

                return RedirectToAction("index");
            }
        }

        [Area("Loja")]
        public PartialViewResult AumentarQtde(int id)
        {
            Carrinho carrinho;
            Estoque estoque;
            List<Estoque> estoqueProds;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");

            ItemPedido itemPed = carrinho.ItensPedido.Find(x => x.Produto.Id == id);//Busca o produto no carrinho

            estoque = new Estoque
            {
                ProdutoId = id
            };
            resultado = new Facade().Consultar(estoque);//Consulta estoque
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return PartialView("_produtos");
            }
            else
            {
                estoqueProds = new List<Estoque>();
                foreach (var item in resultado.Entidades)
                {
                    estoqueProds.Add((Estoque)item);
                }
                if (estoqueProds.FirstOrDefault().Qtde >= carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde + 1)
                {
                    carrinho.ItensPedido[carrinho.ItensPedido.IndexOf(itemPed)].Qtde += 1;//Se sim, incrementa qtde em 1
                    carrinho.QtdeTotalItens += 1;
                }
            }

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
        public IActionResult RemoverItem(int id)
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
            if (carrinho.ItensPedido.Count() > 0)
                return PartialView("_resumo", carrinho.ItensPedido);

            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", new Carrinho());
            return null;
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
        public PartialViewResult CalcularFrete(int id)
        {
            Carrinho carrinho;
            List<Endereco> enderecos;
            Endereco endereco = new Endereco()
            {
                Id = id
            };

            resultado = new Facade().Consultar(endereco);//Busca enderecos
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return PartialView(null);
            }
            else
            {
                enderecos = new List<Endereco>();
                foreach (var item in resultado.Entidades)
                {
                    enderecos.Add((Endereco)item);
                }
                carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
                carrinho.Cep = enderecos.FirstOrDefault().Cep;
                carrinho.EnderecoId = enderecos.FirstOrDefault().Id;
                carrinho.ValorFrete = CalculoFrete.Calcular(carrinho.Cep, carrinho.QtdeTotalItens);
                SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);
                return PartialView("_resumo", carrinho);
            }
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
        public IActionResult EnviarPedido(CartaoDeCredito cartaoUm, CartaoDeCredito cartaoDois, byte multiplosCartoes)
        {
            Carrinho carrinho;
            Pedido pedido;

            carrinho = SessionHelper.Get<Carrinho>(HttpContext.Session, "carrinho");
            
            pedido = new Pedido
            {
                MultiplosCartoes = carrinho.MultiplosCartoes = multiplosCartoes,
                ItensPedido = carrinho.ItensPedido,
                CuponsTroca = carrinho.CuponsTroca,
                CupomPromocional = carrinho.CupomPromocional,
                ValorFrete = carrinho.ValorFrete,
                CartaoUm = carrinho.CartaoUm = cartaoUm,
                CartaoDois = carrinho.CartaoDois = cartaoDois,
                EnderecoId = carrinho.EnderecoId,
                UsuarioId = HttpContext.Session.Get<int>("idUsuario")
            };

            SessionHelper.Set<Carrinho>(HttpContext.Session, "carrinho", carrinho);

            resultado = new Facade().Salvar(pedido);//Salva o pedido
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                string msg = resultado.Msg;
                HttpContext.Session.Set("mensagemCarrinho", Encoding.UTF8.GetBytes(msg));
                return RedirectToAction("Index", "Carrinho");
            }

            resultado = new Facade().Alterar(pedido);//Alterar o pedido
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                string msg = resultado.Msg;
                HttpContext.Session.Set("mensagemCarrinho", Encoding.UTF8.GetBytes(msg));
                return RedirectToAction("Index", "Carrinho");
            }
            if (pedido.Status == 'A')
            {
                string msg;
                if (pedido.CupomTrocaGerado.Id > 0)
                    msg = "Pedido " + pedido.Id + " realizado com sucesso.\nCupom de troca gerado no valor de R$ " + pedido.CupomTrocaGerado.Valor + ".";
                else
                    msg = "Pedido " + pedido.Id + " realizado com sucesso";
                char statusPedido = 'A';
                HttpContext.Session.Set("mensagemCarrinho", Encoding.UTF8.GetBytes(msg));
                HttpContext.Session.Set("statusPedido", statusPedido);
            }
            else if (pedido.Status == 'R')
            {
                string msg = "Pedido " + pedido.Id + " Recusado.\n Contate a operadora de cartão de crédito";
                char statusPedido = 'R';
                HttpContext.Session.Set("mensagemCarrinho", Encoding.UTF8.GetBytes(msg));
                HttpContext.Session.Set("statusPedido", statusPedido);
            }
            else
            {
                string msg = "Pedido efetuado com sucesso";
                char statusPedido = 'P';
                HttpContext.Session.Set("mensagemCarrinho", Encoding.UTF8.GetBytes(msg));
                HttpContext.Session.Set("statusPedido", statusPedido);
            }

            return RedirectToAction("Index", "Carrinho");
        }
    }
}