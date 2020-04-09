using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosPedido : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                if (pedido.ItensPedido.Count == 0)
                    return "Um pedido precisa ter pelo menos um item";
                foreach (var item in pedido.ItensPedido)
                {
                    if (item.Produto.Id <= 0)
                        return "O pedido possui item(ns) inválido(s)";
                    if (item.Qtde <= 0)
                        return "O pedido possui item(ns) com quantidade(s) inválida(s)";
                    if (item.Produto.PrecoVenda <= 0)
                        return "O pedido possui item(ns) com valor(es) inválido(s)";
                }
                if (pedido.EnderecoId <= 0)
                    return "Endereço de entrega não informado";
                if (pedido.ValorFrete <= 0)
                    return "Valor de frete inválido";
                if (pedido.MultiplosCartoes == 1)
                {
                    if (pedido.CartaoUm.Id < 1 || pedido.CartaoDois.Id < 1)
                        return "Cartão de crédito não informado";
                    if (pedido.CartaoUm.CodigoSeguranca == null || pedido.CartaoDois.CodigoSeguranca == null)
                        return "Código de segurança do cartão não informado";
                    if (string.IsNullOrEmpty(pedido.CartaoUm.Validade) || string.IsNullOrWhiteSpace(pedido.CartaoUm.Validade) ||
                        string.IsNullOrEmpty(pedido.CartaoDois.Validade) || string.IsNullOrWhiteSpace(pedido.CartaoDois.Validade))
                        return "Validade do cartão não informada";
                    else if (pedido.CartaoUm.Validade.Length != 7 || !pedido.CartaoUm.Validade.Contains('/') || Int32.TryParse(pedido.CartaoUm.Validade, out _) ||
                        pedido.CartaoDois.Validade.Length != 7 || !pedido.CartaoDois.Validade.Contains('/') || Int32.TryParse(pedido.CartaoDois.Validade, out _))
                        return "Validade do cartão em formato inválido";
                    if (pedido.CartaoUm.Valor == null || pedido.CartaoDois.Valor == null)
                        return "Informe o valor a ser cobrado em cada cartão";
                }
                else if (pedido.MultiplosCartoes == 0)
                {
                    if (pedido.CartaoUm.Id < 1)
                    {
                        if (pedido.ValorTotalPedido > 0.00)
                            return "Cartão de crédito não informado";
                    }
                    if (pedido.CartaoUm.Id < 1 && pedido.CartaoDois.Id > 0)
                        return "Preencha somente o primeiro cartão.";
                    if (pedido.CartaoUm.Id > 0 && pedido.CartaoDois.Id > 0)
                        return "Para compra com mais de um cartão, por favor marque a opção:\nPagar com mais de um cartão de crédito";
                    if (string.IsNullOrEmpty(pedido.CartaoUm.Validade) || string.IsNullOrWhiteSpace(pedido.CartaoUm.Validade))
                        return "Validade do cartão não informada";
                    else if (pedido.CartaoUm.Validade.Length != 7 || !pedido.CartaoUm.Validade.Contains('/') || Int32.TryParse(pedido.CartaoUm.Validade, out _))
                        return "Validade do cartão em formato inválido";
                    if (pedido.CartaoUm.CodigoSeguranca == null)
                        return "Código de segurança do cartão não informado";
                }
                else
                    return "Erro referente a opção de múltiplos cartões";
                if (pedido.UsuarioId < 1)
                    return "Usuário não informado";
            }
            else
            {
                return "Deve ser registrado um pedido";
            }
            return null;
        }
    }
}
