using Core.Interfaces;
using Domain;
using Domain.Negocio;

namespace Core.Impl.Business
{
    public class ValidadorValorMinimoCartao : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                if (pedido.CupomPromocional.Id < 1 && pedido.CuponsTroca.Count == 0)
                {
                    if (pedido.MultiplosCartoes == 1)
                    {
                        if (pedido.CartaoUm.Valor < 10 || pedido.CartaoDois.Valor < 10)
                            return "O valor mínimo para pagamento com múltiplos cartões é de\n R$ 10,00 em cada cartão";
                    }
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
