using Core.Interfaces;
using Domain;
using Domain.DadosCliente;
using System;

namespace Core.Impl.Business
{
    public class ValidadorDataNascimento : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;

                if (!DateTime.TryParse(usuario.DataNascimento, out _))
                    return "Data de nascimento inválida";
            }
            else
            {
                return "Deve ser registrado um usuário";
            }
            return null;
        }
    }
}
