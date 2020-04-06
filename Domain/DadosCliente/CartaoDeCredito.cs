using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DadosCliente
{
    public class CartaoDeCredito : EntidadeDominio
    {
        public byte Bandeira { get; set; }
        public string Numeracao { get; set; }
        public string NomeImpresso { get; set; }
        public string Validade { get; set; }
        public int? CodigoSeguranca { get; set; }
        public int UsuarioId { get; set; }
        public int QtdeParcelas { get; set; }
        public double Valor { get; set; }
    }
}
