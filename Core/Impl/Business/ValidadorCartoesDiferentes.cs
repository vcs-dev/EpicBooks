using Core.Interfaces;
using Domain;
using Domain.Negocio;

namespace Core.Impl.Business
{
    public class ValidadorCartoesDiferentes : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                if (pedido.CartaoUm.Id > 0 && pedido.CartaoDois.Id > 0)
                {
                    if (pedido.CartaoUm.Id == pedido.CartaoDois.Id)
                        return "Os cartões devem ser diferentes";
                }
            }
            else
            {
                return "Deve ser registrado um pedido";
            }
            return null;
        }
    }
}
