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
                char[] usuarioArray = usuario.Cpf.ToCharArray();

                if(usuario.Cpf.Length < 14 || !usuario.Cpf.Contains('-') || !usuarioArray[3].Equals('.') || !usuarioArray[7].Equals('.'))
                    return "CPF inválido";
            }
            else
            {
                return "Deve ser registrado um usuário";
            }
            return null;
        }
    }
}
