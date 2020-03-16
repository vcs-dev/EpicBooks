using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Produto
{
    public class Livro: EntidadeDominio
    {
        public Livro()
        {
            Autores = new List<int>();
            Generos = new List<int>();
            Status = 0;
            AlterarImagem = false;
        }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public IFormFile Imagem { get; set; }
        public string CaminhoImagem { get; set; }
        public string CaminhoImagemBackup { get; set; }
        public IList<int> Generos { get; set; }
        public IList<int> Autores { get; set; }
        public int? Editora { get; set; }
        public string AnoLancamento { get; set; }
        public int? QtdePaginas { get; set; }
        public int? Edicao { get; set; }
        public int? Volume { get; set; }
        public int? Peso { get; set; }
        public decimal? Altura { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Largura { get; set; }
        public int TipoCapa { get; set; }
        public string Isbn { get; set; }
        public int GrupoPrecificacao { get; set; }
        public byte Status { get; set; }
        public string MotivoMudancaStatus { get; set; }
        public int? CategoriaAtivacao { get; set; }
        public int? CategoriaInativacao { get; set; }
        public string PrecoVenda { get; set; }
        public bool AlterarImagem { get; set; }
    }
}
