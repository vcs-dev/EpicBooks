using Core.Interfaces;
using Domain;
using Domain.DadosCliente;
using System.Text.RegularExpressions;

namespace Core.Impl.Business
{
    public class ValidadorDadosEndereco : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            Regex regex;
            MatchCollection mc;

            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;
                regex = new Regex(@"[!@#$%&*()]+");
                mc = regex.Matches(usuario.EnderecoCobranca.Logradouro);
                if (mc.Count < 1)
                    return "O logradouro não deve conter caracteres especiais";

                mc = regex.Matches(usuario.EnderecoEntrega.Logradouro);
                if (mc.Count < 1)
                    return "O logradouro não deve conter caracteres especiais";
            }
            else if (entidade.GetType().Name.Equals("Endereco"))
            {
                Endereco endereco = (Endereco)entidade;
                if (!string.IsNullOrEmpty(endereco.Logradouro))
                {
                    regex = new Regex(@"[!@#$%&*()]+");
                    mc = regex.Matches(endereco.Logradouro);
                    if (mc.Count > 1)
                        return "O logradouro não deve conter caracteres especiais";
                }
            }
            else
            {
                return "Deve ser registrado um usuário ou endereço";
            }
            return null;
        }
    }
}
