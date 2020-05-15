using Core.Interfaces;
using Domain;
using Domain.DadosCliente;

namespace Core.Impl.Business
{
    public class ValidadorSenha : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;
                if (!string.IsNullOrEmpty(usuario.Senha) && !string.IsNullOrEmpty(usuario.ConfirmacaoSenha))
                {
                    if (!usuario.Senha.Equals(usuario.ConfirmacaoSenha))
                        return "Confirmação de senha diferente da senha";
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
