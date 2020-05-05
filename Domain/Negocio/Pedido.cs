using Domain.DadosCliente;
using System;
using System.Collections.Generic;

namespace Domain.Negocio
{
    public class Pedido : EntidadeDominio
    {
        public Pedido()
        {
            Status = '\0';
            ItensPedido = new List<ItemPedido>();
            CuponsTroca = new List<Cupom>();
            CupomPromocional = new Cupom();
            CupomTrocaGerado = new Cupom();
            CartaoUm = new CartaoDeCredito();
            CartaoDois = new CartaoDeCredito();
            IsAlteracaoGerencial = false;
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
        public string DescricaoStatus { get; set; }
        public string Observacao { get; set; }
        public byte MultiplosCartoes { get; set; }
        public Cupom CupomTrocaGerado { get; set; }
        public bool IsAlteracaoGerencial { get; set; }
    }
}
