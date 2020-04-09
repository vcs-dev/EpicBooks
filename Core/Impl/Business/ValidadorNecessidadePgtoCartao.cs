using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System.Linq;

namespace Core.Impl.Business
{
    public class ValidadorNecessidadePgtoCartao : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                if (pedido.ValorTotalPedido <= 0.00 && pedido.CupomPromocional != null && pedido.CuponsTroca.Count() > 0)
                {
                    if (pedido.CartaoUm.Id > 0 || pedido.CartaoDois.Id > 0)
                        return "Uso desnecessário de cartão de crédito.\nO valor dos cupons é suficiente para pagar a compra";
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
