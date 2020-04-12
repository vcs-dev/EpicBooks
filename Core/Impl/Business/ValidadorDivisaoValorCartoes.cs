using Core.Interfaces;
using Domain;
using Domain.Negocio;

namespace Core.Impl.Business
{
    public class ValidadorDivisaoValorCartoes : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                if (pedido.MultiplosCartoes == 1)
                {
                    if (pedido.ValorTotalPedido > 0 && ((pedido.CartaoUm.Valor + pedido.CartaoDois.Valor) > pedido.ValorTotalPedido))
                        return "A soma dos valores a serem pagos com cada cartão\nnão correspondem ao valor total do pedido";
                }
                else if (pedido.ValorTotalPedido > 0.00)
                    pedido.CartaoUm.Valor = pedido.ValorTotalPedido;
                else
                    pedido.CartaoUm.Valor = null;
            }
            else
            {
                return "Deve ser registrado um pedido";
            }
            return null;
        }
    }
}
