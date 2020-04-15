using Core.Application;
using Core.Impl.Control;
using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Impl.Business
{
    public class ValidadorGeracaoCupomTrocaRecebida : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Troca"))
            {
                Troca troca = (Troca)entidade;

                if(troca.Status == 'A')
                {
                    Result resultado = new Facade().Consultar(new ItemPedido { Id = troca.PedidoId });
                    if (resultado.Msg != null)
                        return "Não foi possível confirmar o recebimento.\n" + resultado.Msg;

                    List<ItemPedido> itensPed = new List<ItemPedido>();
                    foreach (var item in resultado.Entidades)
                    {
                        itensPed.Add((ItemPedido)item);
                    }

                    if (itensPed.Count == 0)
                        return "Erro. Item de pedido não encontrado.";

                    //resultado = new Facade().Consultar(new Pedido { Id = troca.PedidoId });
                    //if (resultado.Msg != null)
                    //    return "Não foi possível confirmar o recebimento.\n" + resultado.Msg;

                    //List<Pedido> pedidos = new List<Pedido>();
                    //foreach (var item in resultado.Entidades)
                    //{
                    //    pedidos.Add((Pedido)item);
                    //}

                    Cupom cupomTroca = new Cupom
                    {
                        UsuarioId = troca.UsuarioId,
                        Tipo = 'T',
                        Valor = /*(*/itensPed.FirstOrDefault().Produto.PrecoVenda * itensPed.FirstOrDefault().Qtde/*) + pedidos.FirstOrDefault().ValorFrete*/,
                        Usado = 0,
                        DataCadastro = DateTime.Now,
                        DataExpiracao = null
                    };

                    cupomTroca.Codigo = "ST";
                    cupomTroca.Codigo += troca.PedidoId;
                    cupomTroca.Codigo += troca.ItemId;
                    cupomTroca.Codigo += DateTime.Now.Year;
                    cupomTroca.Codigo += DateTime.Now.Month;
                    cupomTroca.Codigo += DateTime.Now.Day;

                    troca.CupomTroca = cupomTroca;
                }
            }
            else
            {
                return "Deve ser atualizada uma troca";
            }
            return null;
        }
    }
}
