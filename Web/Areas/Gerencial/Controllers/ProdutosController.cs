using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Produto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Gerencial.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;

        public ProdutosController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
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
            string pastaDestino = "produtos";
            string nomeArquivo = Path.GetRandomFileName();
            if (livro.Imagem.FileName.Contains("jpg"))
                nomeArquivo += ".jpg";
            else
                return BadRequest();

            string caminho_WebRoot = _appEnvironment.WebRootPath;
            string caminhoDestinoArquivo = caminho_WebRoot + "\\img\\uploads\\" + pastaDestino + "\\";
            string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + "\\" + nomeArquivo;
            using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
            {
                await livro.Imagem.CopyToAsync(stream);
            }

            return Ok();
        }
    }
}