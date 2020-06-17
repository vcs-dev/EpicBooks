using Core.Interfaces;
using Domain;
using Domain.DadosCliente;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosCartao : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("CartaoDeCredito"))
            {
                CartaoDeCredito cartao = (CartaoDeCredito)entidade;
                if(string.IsNullOrEmpty(cartao.NomeImpresso) || string.IsNullOrEmpty(cartao.Numeracao) ||
                    string.IsNullOrEmpty(cartao.Validade) || cartao.Bandeira == 0)
                {
                    return "Os campos com * são de preenchimento obrigatório";
                }
            }
            else
            {
                return "Deve ser registrado um cartão";
            }
            return null;
        }
    }
}
