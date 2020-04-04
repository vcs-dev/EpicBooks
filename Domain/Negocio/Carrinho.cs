using System;
using System.Collections.Generic;

namespace Domain.Negocio
{
    public class Carrinho : EntidadeDominio
    {
        public Carrinho()
        {
            //Usuario = new Usuario();
            ItensPedido = new List<ItemPedido>();
            CuponsTroca = new List<Cupom>();
        }
        public List<ItemPedido> ItensPedido { get; set; }
        public List<Cupom> CuponsTroca { get; set; }
        public Cupom CupomPromocional { get; set; }
        //public Usuario Usuario { get; set; }
        public string Cep { get; set; }
        public IList<int> CartoesCredito { get; set; }
        public string Frete { get; set; }
        public int QtdeTotalItens { get; set; }
        public DateTime HoraUltimaInclusao { get; set; }
    }
}
