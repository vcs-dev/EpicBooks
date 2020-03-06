using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Produto
{
    public class Livro: EntidadeDominio
    {
        public Livro()
        {
            Autores = new List<int>();
            Categorias = new List<int>();
        }
        public string Titulo { get; set; }
        public string Sinopse { get; set; }
        public string CaminhoImagem { get; set; }
        public IList<int> Categorias { get; set; }
        public IList<int> Autores { get; set; }
        public int? Editora { get; set; }
        public string AnoLancamento { get; set; }
        public int? QtdePaginas { get; set; }
        public int? Peso { get; set; }
        public int? Edicao { get; set; }
        public int? Volume { get; set; }
        public int? Altura { get; set; }
        public int? Largura { get; set; }
        public int? Espessura { get; set; }
        public int? TipoCapa { get; set; }
        public string Isbn { get; set; }
    }
}
