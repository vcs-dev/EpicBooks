using Domain.DadosCliente;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Negocio
{
    public class Pedido : EntidadeDominio
    {
        public Pedido()
        {
            Status = '\0';
            ItensPedido = new List<ItemPedido>();
            SessaoGuid = Guid.Empty;
        }
        public int UsuarioId { get; set; }
        public int EnderecoId { get; set; }
        public List<ItemPedido> ItensPedido { get; set; }
        public CartaoDeCredito CartaoUm { get; set; }
        public CartaoDeCredito CartaoDois { get; set; }
        public Cupom CupomPromocional { get; set; }
        public List<Cupom> CuponsTroca { get; set; }
        public double ValorFrete { get; set; }
        public double ValorTotalPedido { get; set; }
        public char Status { get; set; }
        public Guid SessaoGuid { get; set; }
    }
}
