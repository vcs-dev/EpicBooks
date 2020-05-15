using Core.Interfaces;
using Domain;
using Domain.DadosCliente;
using System;

namespace Core.Impl.Business
{
    public class ValidadorDadosCartao : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;

                if(!string.IsNullOrEmpty(usuario.Cartao.Numeracao) && !Int32.TryParse(usuario.Cartao.Numeracao, out _))
                    return "Numeração do cartão inválida";
                if (usuario.Cartao.Validade.Length < 7 || !usuario.Cartao.Validade.ToCharArray()[2].Equals('/'))
                    return "Data de validade do cartão inválida";
            }
            else
            {
                return "Deve ser registrado um usuário";
            }
            return null;
        }
    }
}
