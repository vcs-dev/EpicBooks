using Domain.DadosCliente;
using System;
using System.Collections.Generic;

namespace Domain.Negocio
{
    public class Carrinho : EntidadeDominio
    {
        public Carrinho()
        {
            ItensPedido = new List<ItemPedido>();
            CupomPromocional = new Cupom();
            CuponsTroca = new List<Cupom>();
            CartaoUm = new CartaoDeCredito();
            CartaoDois = new CartaoDeCredito();
            Cep = "";
            ValorFrete = 00.00;
        }
        public List<ItemPedido> ItensPedido { get; set; }
        public List<Cupom> CuponsTroca { get; set; }
        public Cupom CupomPromocional { get; set; }
        public string Cep { get; set; }
        public CartaoDeCredito CartaoUm { get; set; }
        public CartaoDeCredito CartaoDois { get; set; }
        public double ValorFrete { get; set; }
        public int QtdeTotalItens { get; set; }
        public DateTime HoraUltimaInclusao { get; set; }
        public int EnderecoId { get; set; }
        public byte MultiplosCartoes { get; set; }
    }
}
