using Core.Interfaces;
using Domain;
using Domain.Negocio;

namespace Core.Impl.Business
{
    public class CalculoValorTotalPedido : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                double valorTotalPedido = 0;
                foreach (var item in pedido.ItensPedido)
                {
                    valorTotalPedido += item.Produto.PrecoVenda * item.Qtde;
                }
                valorTotalPedido += pedido.ValorFrete;

                foreach (var item in pedido.CuponsTroca)
                {
                    valorTotalPedido -= item.Valor;
                }
                valorTotalPedido -= pedido.CupomPromocional.Valor;
                pedido.ValorTotalPedido = valorTotalPedido;
            }
            else
            {
                return "Deve ser registrado um pedido";
            }
            return null;
        }
    }
}
