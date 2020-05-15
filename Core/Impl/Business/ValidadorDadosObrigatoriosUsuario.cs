using Core.Interfaces;
using Domain;
using Domain.DadosCliente;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosUsuario : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;
                if(string.IsNullOrEmpty(usuario.NomeCompleto) || string.IsNullOrEmpty(usuario.DataNascimento) ||
                    usuario.Sexo == 0 || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.ConfirmacaoSenha) ||
                    usuario.TelefoneTipo == 0 || string.IsNullOrEmpty(usuario.TelefoneDdd) ||
                    string.IsNullOrEmpty(usuario.Cartao.NomeImpresso) || string.IsNullOrEmpty(usuario.Cartao.Numeracao) ||
                    string.IsNullOrEmpty(usuario.Cartao.Validade) || usuario.Cartao.Bandeira == 0)
                {
                    return "Os campos com * são de preenchimento obrigatório";
                }
                if(usuario.EndEntregaECobranca == 0)
                {
                    if(usuario.EnderecoEntrega.TipoEndereco == 0 || usuario.EnderecoEntrega.TipoLogradouro == 0 ||
                        usuario.EnderecoEntrega.TipoResidencia == 0 || usuario.EnderecoEntrega.Cidade == 0 ||
                        usuario.EnderecoEntrega.Estado == 0 || usuario.EnderecoEntrega.Pais == 0 ||
                        string.IsNullOrEmpty(usuario.EnderecoEntrega.Logradouro) || usuario.EnderecoEntrega.Numero == 0 ||
                        string.IsNullOrEmpty(usuario.EnderecoEntrega.Bairro) || string.IsNullOrEmpty(usuario.EnderecoEntrega.Cep))
                    {
                        return "Os campos com * são de preenchimento obrigatório";
                    }
                    if (usuario.EnderecoCobranca.TipoEndereco == 0 || usuario.EnderecoCobranca.TipoLogradouro == 0 ||
                        usuario.EnderecoCobranca.TipoResidencia == 0 || usuario.EnderecoCobranca.Cidade == 0 ||
                        usuario.EnderecoCobranca.Estado == 0 || usuario.EnderecoCobranca.Pais == 0 ||
                        string.IsNullOrEmpty(usuario.EnderecoCobranca.Logradouro) || usuario.EnderecoCobranca.Numero == 0 ||
                        string.IsNullOrEmpty(usuario.EnderecoCobranca.Bairro) || string.IsNullOrEmpty(usuario.EnderecoCobranca.Cep))
                    {
                        return "Os campos com * são de preenchimento obrigatório";
                    }
                }
                else
                {
                    if (usuario.EnderecoEntrega.TipoEndereco == 0 || usuario.EnderecoEntrega.TipoLogradouro == 0 ||
                        usuario.EnderecoEntrega.TipoResidencia == 0 || usuario.EnderecoEntrega.Cidade == 0 ||
                        usuario.EnderecoEntrega.Estado == 0 || usuario.EnderecoEntrega.Pais == 0 ||
                        string.IsNullOrEmpty(usuario.EnderecoEntrega.Logradouro) || usuario.EnderecoEntrega.Numero == 0 ||
                        string.IsNullOrEmpty(usuario.EnderecoEntrega.Bairro) || string.IsNullOrEmpty(usuario.EnderecoEntrega.Cep))
                    {
                        return "Os campos com * são de preenchimento obrigatório";
                    }
                }
            }
            else
            {
                return "Deve ser registrado um usuário";
            }
            return null;
        }
    }
}
