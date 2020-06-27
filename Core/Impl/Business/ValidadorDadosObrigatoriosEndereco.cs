using Core.Interfaces;
using Domain;
using Domain.DadosCliente;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosEndereco : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Endereco"))
            {
                Endereco endereco = (Endereco)entidade;
                if(endereco.TipoEndereco < 1 || endereco.TipoLogradouro < 1 || endereco.TipoResidencia < 1 ||
                    endereco.Cidade < 1 || endereco.Estado < 1 || endereco.Pais < 1 || endereco.Numero == null || endereco.Numero < 1 ||
                    string.IsNullOrEmpty(endereco.Logradouro))
                {
                    return "Os campos com * são de preenchimento obrigatório";
                }
            }
            else
            {
                return "Deve ser registrado um endereço";
            }
            return null;
        }
    }
}
