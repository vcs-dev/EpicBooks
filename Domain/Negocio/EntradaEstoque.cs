using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Negocio
{
    public class EntradaEstoque : EntidadeDominio
    {
        public int ProdutoId { get; set; }
        public int Qtde { get; set; }
        public double ValorCusto { get; set; }
        public int FornecedorId { get; set; }
        public string Observacao { get; set; }
    }
}
