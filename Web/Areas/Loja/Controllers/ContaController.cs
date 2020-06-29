using Core.Application;
using Core.Impl.Control;
using Domain.DadosCliente;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Util;

namespace Web.Areas.Loja.Controllers
{
    public class ContaController : Controller
    {
        private Result resultado;

        [Area("Loja")]
        public IActionResult Login()
        {
            return View();
        }

        [Area("Loja")]
        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            resultado = new Facade().Consultar(new Usuario { Email = email, Senha = senha });
            if(resultado.Msg != null)
                ViewBag.Mensagem = resultado.Msg;
            else
            {
                if(resultado.Entidades.Count == 0)
                    ViewBag.Mensagem = "Usuário não encontrado.";
                else
                {
                    List<Usuario> usuarios = new List<Usuario>();
                    foreach (var entidade in resultado.Entidades)
                    {
                        usuarios.Add((Usuario)entidade);
                    }
                    if (usuarios.FirstOrDefault().Ativo > 0)
                    {
                        HttpContext.Session.Set("nomeUsuario", Encoding.UTF8.GetBytes(usuarios.FirstOrDefault().NomeCompleto));
                        HttpContext.Session.Set("idUsuario", Encoding.UTF8.GetBytes(usuarios.FirstOrDefault().Id.ToString()));
                        return RedirectToAction("Index", "MinhaConta");
                    }
                    else
                        ViewBag.Mensagem = "Conta inativada pelo titular. Para reativá-la contate o suporte técnico.";
                }
            }
            return View();
        }

        [Area("Loja")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("nomeUsuario");
            HttpContext.Session.Remove("idUsuario");
            return RedirectToAction("Index", "Home");
        }

        [Area("Loja")]
        public IActionResult Cadastrar()
        {
            return View(new Usuario());
        }

        [Area("Loja")]
        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            resultado = new Facade().Salvar(usuario);
            if(resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View(usuario);
            }
            ViewBag.Mensagem = "Cadastro efetuado com sucesso!";
            return View(new Usuario());
        }
    }
}