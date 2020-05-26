using Core.Interfaces;
using Domain;
using Domain.DadosCliente;

namespace Core.Impl.Business
{
    public class ValidadorEmail : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;
                if (!string.IsNullOrEmpty(usuario.Email))
                {
                    if (!usuario.Email.Contains('@') || !usuario.Email.Contains(".com"))
                        return "E-Mail inválido";
                    if (usuario.Email.IndexOf(".com") < usuario.Email.IndexOf('@'))
                        return "E-Mail inválido";
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
