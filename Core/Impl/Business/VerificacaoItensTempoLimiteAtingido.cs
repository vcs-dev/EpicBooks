using Core.Impl.DAO.Negocio;
using Domain.Negocio;
using System;
using System.Collections.Generic;

namespace Core.Impl.Business
{
    public class VerificacaoItensTempoLimiteAtingido
    {
        public string VerificarItensCarrinho(Carrinho carrinho)
        {
            TimeSpan timeSpan;
            ItemBoqueadoDAO itemBloqueadoDAO = new ItemBoqueadoDAO();
            //List<ItemPedido> itensPed = new List<ItemPedido>();
            int param = 15;
            if (carrinho.HoraUltimaInclusao != DateTime.MinValue)
            {
                timeSpan = DateTime.Now.Subtract(carrinho.HoraUltimaInclusao);
                if (timeSpan.Minutes >= param)
                {
                    foreach (var item in carrinho.ItensPedido)
                    {
                        item.Id = item.Produto.Id;//Pois o DAO de exclusao pega EntidadeDominio.Id
                        itemBloqueadoDAO.Excluir(item);
                    }
                    return "Limite de tempo para finalização de compra atingido. Refaça seu pedido.";
                }
            }
            //foreach (var item in carrinho.ItensPedido)
            //{
            //    timeSpan = carrinho.HoraUltimaInclusao.Subtract(item.HoraInclusao);
            //    if (timeSpan.Minutes >= param)
            //    {
            //        item.Id = item.Produto.Id;//Pois o DAO de exclusao pega EntidadeDominio.Id
            //        itensPed.Add(item);
            //    }
            //}
            //foreach (var item in itensPed)
            //{
            //    carrinho.ItensPedido.Remove(item);
            //}
            //foreach (var item in itensPed)
            //{
            //    itemBloqueadoDAO.Excluir(item);
            //}
            //if (itensPed.Count > 0)
            //    return "Um ou mais produtos foram removidos de seu carrinho.\nMotivo: Mais de " + param + " minutos desde a última inclusão no carrinho.";
            return null;
        }
    }
}
