using Core.Interfaces;
using Domain;
using Domain.DadosCliente;
using System.Text.RegularExpressions;

namespace Core.Impl.Business
{
    public class ValidadorTrocaSenha : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;
                Regex regex;
                MatchCollection mc;

                if (!string.IsNullOrEmpty(usuario.NovaSenha) && !string.IsNullOrEmpty(usuario.ConfirmacaoSenha) &&
                    usuario.DadosAlterados.Equals("SENHA"))
                {
                    if (!usuario.NovaSenha.Equals(usuario.ConfirmacaoSenha))
                        return "Confirmação de senha diferente da nova senha";

                    if (string.IsNullOrWhiteSpace(usuario.NovaSenha))
                    {
                        return "A nova senha não pode ter espaços em branco";
                    }
                    if (usuario.NovaSenha.Length < 8)
                        return "A nova senha deve conter pelo menos 8 caracteres";

                    regex = new Regex(@"[0-9]+");
                    mc = regex.Matches(usuario.NovaSenha);
                    if (mc.Count < 1)
                        return "A nova senha deve conter letras maiúsculas, minúsculas e caracteres especiais";
                    regex = new Regex(@"[a-z]+");
                    mc = regex.Matches(usuario.NovaSenha);
                    if (mc.Count < 1)
                        return "A nova senha deve conter letras maiúsculas, minúsculas e caracteres especiais";
                    regex = new Regex(@"[A-Z]+");
                    mc = regex.Matches(usuario.NovaSenha);
                    if (mc.Count < 1)
                        return "A nova senha deve conter letras maiúsculas, minúsculas e caracteres especiais";
                    regex = new Regex(@"[!@#$%&*()]+");
                    mc = regex.Matches(usuario.NovaSenha);
                    if (mc.Count < 1)
                        return "A nova senha deve conter letras maiúsculas, minúsculas e caracteres especiais";
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
