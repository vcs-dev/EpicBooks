using Core.Application;
using Core.Impl.Control;
using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Impl.Business
{
    class ValidadorQtdeTrocaCompativelComPedido : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Troca"))
            {
                Result resultado;
                Troca troca = (Troca)entidade;
                List<Pedido> pedidos;

                resultado = new Facade().Consultar(new Pedido { Id = troca.PedidoId });
                if (resultado.Msg != null)
                    return "Não foi possível validar a troca";

                pedidos = new List<Pedido>();
                foreach (var item in resultado.Entidades)
                {
                    pedidos.Add((Pedido)item);
                }
                ItemPedido temp;
                temp = pedidos.FirstOrDefault().ItensPedido.Find(x => x.Id == troca.ItemId);
                if (temp == null)
                    return "Item do pedido não encontrado";
                if (troca.Qtde > temp.Qtde)
                    return "Quantidade informada superior à quantidade comprada";
            }
            else
            {
                return "Deve ser registrado um livro";
            }
            return null;
        }
    }
}
