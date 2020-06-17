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
                if (!string.IsNullOrEmpty(usuario.Cartao.Numeracao) && !string.IsNullOrEmpty(usuario.Cartao.Validade))
                {
                    if (!string.IsNullOrEmpty(usuario.Cartao.Numeracao) && !Int64.TryParse(usuario.Cartao.Numeracao, out _))
                        return "Numeração do cartão inválida";
                    if(usuario.Cartao.Numeracao.Length < 16)
                        return "A numeração do cartão deve possuir 16 dígitos";
                    if (usuario.Cartao.Validade.Length < 7 || !usuario.Cartao.Validade.ToCharArray()[2].Equals('/'))
                        return "Data de validade do cartão inválida";
                }
            }
            else if (entidade.GetType().Name.Equals("CartaoDeCredito"))
            {
                CartaoDeCredito cartao = (CartaoDeCredito)entidade;
                if (!string.IsNullOrEmpty(cartao.Numeracao) && !string.IsNullOrEmpty(cartao.Validade))
                {
                    if (!string.IsNullOrEmpty(cartao.Numeracao) && !Int64.TryParse(cartao.Numeracao, out _))
                        return "Numeração do cartão inválida";
                    if (cartao.Numeracao.Length < 16)
                        return "A numeração do cartão deve possuir 16 dígitos";
                    if (cartao.Validade.Length < 7 || !cartao.Validade.ToCharArray()[2].Equals('/'))
                        return "Data de validade do cartão inválida";
                }
            }
            else
            {
                return "Deve ser registrado um usuário ou cartão";
            }
            return null;
        }
    }
}
