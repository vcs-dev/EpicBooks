using Domain.DadosCliente;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Negocio
{
    public class Pedido : EntidadeDominio
    {
        public int IdUsuario { get; set; }
        public int IdEndereco { get; set; }
        public ItemPedido ItensPedido { get; set; }
        public CartaoDeCredito CartaoUm { get; set; }
        public CartaoDeCredito CartaoDois { get; set; }
        public Cupom CupomPromocional { get; set; }
        public List<Cupom> CuponsTroca { get; set; }
        public double ValorFrete { get; set; }
        public double ValorTotalPedido { get; set; }
    }
}
