using Domain.Produto;
using System;

namespace Domain.Negocio
{
    public class ItemPedido : EntidadeDominio
    {
        public ItemPedido()
        {
            IsBloqueado = false;
            Produto = new Livro();
        }
        public Livro Produto { get; set; }
        public int Qtde { get; set; }
        public DateTime HoraInclusao { get; set; }
        public bool IsBloqueado { get; set; }
    }
}
