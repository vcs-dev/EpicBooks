using Core.Interfaces;
using Domain;
using Domain.DadosCliente;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosTrocaSenha : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;
                if (usuario.DadosAlterados.Equals("SENHA"))
                {
                    if (string.IsNullOrEmpty(usuario.Senha))
                        return "Informe a senha atual";
                    if (string.IsNullOrEmpty(usuario.NovaSenha))
                        return "Informe a nova senha";
                    if (string.IsNullOrEmpty(usuario.ConfirmacaoSenha))
                        return "Confirme a nova senha";
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
