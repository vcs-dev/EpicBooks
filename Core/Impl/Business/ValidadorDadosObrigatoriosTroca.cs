using Core.Interfaces;
using Domain;
using Domain.Negocio;

namespace Core.Impl.Business
{
    class ValidadorDadosObrigatoriosTroca : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Troca"))
            {
                Troca troca = (Troca)entidade;
                if (troca.ItemId == 0)
                    return "O ID do item não pode ser nulo";
                if (troca.PedidoId == 0)
                    return "O ID do pedido não pode ser nulo";
                if (troca.Qtde <= 0)
                    return "Informe uma quantidade";
                if (troca.Status == '\0')
                    return "O status da troca não pode ser nulo";
            }
            else
            {
                return "Deve ser registrada uma troca";
            }
            return null;
        }
    }
}
