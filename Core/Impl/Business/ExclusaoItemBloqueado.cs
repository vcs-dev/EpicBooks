using Core.Impl.DAO.Negocio;
using Domain.Negocio;

namespace Core.Impl.Business
{
    public class ExclusaoItemBloqueado
    {
        public void Excluir(ItemPedido itemPed)
        {
            itemPed.Id = itemPed.Produto.Id;
            ItemBoqueadoDAO itemBoqueadoDAO = new ItemBoqueadoDAO();
            itemBoqueadoDAO.Excluir(itemPed);
        }
    }
}
