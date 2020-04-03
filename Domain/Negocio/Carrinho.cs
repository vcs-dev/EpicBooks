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
            Cep = "00000-000";
            //Cupons = new List<Cupom>();
        }
        public int Id { get; set; }
        public IList<ItemPedido> ItensPedido { get; set; }
        //public IList<Cupom> Cupons { get; set; }
        //public Usuario Usuario { get; set; }
        public string Cep { get; set; }
        public IList<int> CartoesCredito { get; set; }
        public string Frete { get; set; }
        public int QtdeTotalItens { get; set; }
        public DateTime HoraUltimaInclusao { get; set; }
    }
}
