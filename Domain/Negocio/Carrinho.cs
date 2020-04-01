using System.Collections.Generic;

namespace Domain.Negocio
{
    public class Carrinho
    {
        public Carrinho()
        {
            //Usuario = new Usuario();
            ItensPedido = new List<ItemPedido>();
            //Cupons = new List<Cupom>();
        }
        public int Id { get; set; }
        public List<ItemPedido> ItensPedido { get; set; }
        //public List<Cupom> Cupons { get; set; }
        //public Usuario Usuario { get; set; }
        //public CartaoCredito CartaoAdicional { get; set; }
    }
}
