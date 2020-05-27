using Core.Interfaces;
using Domain;
using Domain.DadosCliente;
using System;

namespace Core.Impl.Business
{
    public class ValidadorCpf : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;

                if (!string.IsNullOrEmpty(usuario.Cpf))
                {
                    if (usuario.Cpf.Length < 11 || !Int64.TryParse(usuario.Cpf, out _))
                        return "CPF inválido";
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
