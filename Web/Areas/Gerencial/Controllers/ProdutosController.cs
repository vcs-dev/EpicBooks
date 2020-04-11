using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Application;
using Core.Impl.Control;
using Domain.Produto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Gerencial.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private Result resultado;

        public ProdutosController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [Area("Gerencial")]
        public IActionResult Index()
        {
            resultado = new Facade().Consultar(new Livro());
            if (resultado.Msg != null)
            {
                ViewBag.Mensagem = resultado.Msg;
                return View(new List<Livro>());
            }
            if (resultado.Entidades.Count == 0)
            {
                ViewBag.Mensagem = "Não existem produtos cadastrados.";
                return View(new List<Livro>());
            }
            List<Livro> livros = new List<Livro>();
            foreach (var item in resultado.Entidades)
            {
                livros.Add((Livro)item);
            }

            return View(livros);
        }

        [Area("Gerencial")]
        public IActionResult Cadastrar()
        {
            return View(new Livro());
        }

        [Area("Gerencial")]
        [HttpPost]
        public async Task<IActionResult> CadastrarAsync(Livro livro)
        {
            resultado = new Facade().Consultar(livro);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                string pastaDestino = "produtos";
                string caminhoWebRoot = _appEnvironment.WebRootPath;
                string nomeArquivo = Path.GetRandomFileName();
                if (livro.Imagem.FileName.Contains("jpg"))
                    nomeArquivo += ".jpg";
                else
                    return BadRequest();
                string caminhoCurtoENomeArquivo = "\\img\\uploads\\" + pastaDestino + "\\" + nomeArquivo;
                FileInfo fi = new FileInfo(caminhoCurtoENomeArquivo);
                while (fi.Exists)
                {
                    nomeArquivo = Path.GetRandomFileName();
                    if (livro.Imagem.FileName.Contains("jpg"))
                        nomeArquivo += ".jpg";
                    else
                        return BadRequest();
                    caminhoCurtoENomeArquivo = "\\img\\uploads\\" + pastaDestino + "\\" + nomeArquivo;
                }
                string caminhoDestinoArquivoCompleto = caminhoWebRoot + caminhoCurtoENomeArquivo;
                using (var stream = new FileStream(caminhoDestinoArquivoCompleto, FileMode.Create))
                {
                    await livro.Imagem.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(caminhoCurtoENomeArquivo.Trim()))
                    livro.CaminhoImagem = caminhoCurtoENomeArquivo.Replace("\\", "/");

                resultado = new Facade().Salvar(livro);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    TempData["MsgErro"] = resultado.Msg;
                    return View();
                }
                else
                {
                    TempData["MsgSucesso"] = resultado.Msg;
                    return RedirectToAction("Index", "Produtos", new { area = "Gerencial" });
                }
            }
            else
            {
                TempData["MsgErro"] = "Isbn já cadastrado.";
                return View("index");
            }
        }

        [Area("Gerencial")]
        public IActionResult Detalhes(int id)
        {
            Livro livro = new Livro();
            List<Livro> livros;
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
                return View(livros.FirstOrDefault());
            }
        }

        [Area("Gerencial")]
        public IActionResult Editar(int id)
        {
            Livro livro = new Livro();
            List<Livro> livros;
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
                livros.FirstOrDefault().CaminhoImagemBackup = livros.FirstOrDefault().CaminhoImagem;
                return View(livros.FirstOrDefault());
            }
        }

        [Area("Gerencial")]
        [HttpPost]
        public async Task<IActionResult> EditarAsync(Livro livro)
        {
            Livro livroConsulta = new Livro();
            List<Livro> livros;
            livroConsulta.Id = livro.Id;
            string caminhoWebRoot = _appEnvironment.WebRootPath;
            string caminhoImagemAntiga = livro.CaminhoImagemBackup.Replace("/", "\\");
            resultado = new Facade().Consultar(livroConsulta);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return View("editar", livro.Id);
            }
            else
            {
                TempData["MsgSucesso"] = resultado.Msg;
                livros = new List<Livro>();
                foreach (var item in resultado.Entidades)
                {
                    livros.Add((Livro)item);
                }
            }
            if (livro.Imagem == null)
                livro.CaminhoImagem = livro.CaminhoImagemBackup;
            else
            {
                string pastaDestino = "produtos";
                string nomeArquivo = Path.GetRandomFileName();
                if (livro.Imagem.FileName.Contains("jpg"))
                    nomeArquivo += ".jpg";
                else
                    return BadRequest();
                string caminhoCurtoENomeArquivo = "\\img\\uploads\\" + pastaDestino + "\\" + nomeArquivo;
                FileInfo fInfo = new FileInfo(caminhoCurtoENomeArquivo);
                while (fInfo.Exists)
                {
                    nomeArquivo = Path.GetRandomFileName();
                    if (livro.Imagem.FileName.Contains("jpg"))
                        nomeArquivo += ".jpg";
                    else
                        return BadRequest();
                    caminhoCurtoENomeArquivo = "\\img\\uploads\\" + pastaDestino + "\\" + nomeArquivo;
                }
                string caminhoDestinoArquivoCompleto = caminhoWebRoot + caminhoCurtoENomeArquivo;
                using (var stream = new FileStream(caminhoDestinoArquivoCompleto, FileMode.Create))
                {
                    await livro.Imagem.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(caminhoCurtoENomeArquivo.Trim()))
                    livro.CaminhoImagem = caminhoCurtoENomeArquivo.Replace("\\", "/");
            }
            resultado = new Facade().Alterar(livro);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return View("editar", livro.Id);
            }
            else
            {
                TempData["MsgSucesso"] = resultado.Msg;
                string temp = caminhoImagemAntiga.Replace("\\", "/");
                if (!temp.Equals(livro.CaminhoImagem))
                {
                    FileInfo fi = new FileInfo(caminhoWebRoot + caminhoImagemAntiga);
                    fi.Delete();
                }
                return RedirectToAction("Index", "Produtos", new { area = "Gerencial" });
            }
        }
        [Area("Gerencial")]
        public PartialViewResult Consultar(string stringBusca)
        {
            if (string.IsNullOrEmpty(stringBusca) || string.IsNullOrWhiteSpace(stringBusca))
                return PartialView("_produtos");

            List<Livro> livros;
            Livro livro = new Livro
            {
                Nome = stringBusca
            };

            resultado = new Facade().Consultar(livro);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                TempData["MsgErro"] = resultado.Msg;
                return PartialView("_produtos");
            }
            else
            {
                TempData["MsgSucesso"] = resultado.Msg;
                livros = new List<Livro>();
                foreach (var item in resultado.Entidades)
                {
                    livros.Add((Livro)item);
                }
                return PartialView("_produtos", livros);
            }
        }
    }
}

