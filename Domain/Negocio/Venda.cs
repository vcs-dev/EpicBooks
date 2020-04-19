using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Negocio
{
    public class Venda : EntidadeDominio
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string NomeProduto { get; set; }
        public int Qtde { get; set; }
        public string Data { get; set; }
        public string TipoGrafico { get; set; }
    }
}
