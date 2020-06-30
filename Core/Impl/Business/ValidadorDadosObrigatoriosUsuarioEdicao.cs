using Core.Interfaces;
using Domain;
using Domain.DadosCliente;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosUsuarioEdicao : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;
                if (usuario.DadosAlterados.Equals("SENHA"))
                {
                    if (string.IsNullOrEmpty(usuario.Senha) || string.IsNullOrWhiteSpace(usuario.Senha) ||
                        string.IsNullOrEmpty(usuario.ConfirmacaoSenha) || string.IsNullOrWhiteSpace(usuario.ConfirmacaoSenha) ||
                        string.IsNullOrEmpty(usuario.NovaSenha) || string.IsNullOrWhiteSpace(usuario.NovaSenha))
                        return "Os campos com * são de preenchimento obrigatório";
                }
                else
                {
                    if (string.IsNullOrEmpty(usuario.NomeCompleto) || string.IsNullOrEmpty(usuario.DataNascimento) ||
                        usuario.Sexo == 0 || string.IsNullOrEmpty(usuario.Email) || usuario.TelefoneTipo == 0 ||
                        string.IsNullOrEmpty(usuario.TelefoneDdd) || string.IsNullOrEmpty(usuario.TelefoneDdi) ||
                        string.IsNullOrEmpty(usuario.TelefoneNumero))
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
