using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;

namespace Core.Impl.Business
{
    public class VerificadorNecessidadeGeracaoCupomTroca : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                if(pedido.ValorTotalPedido < 0.00)
                {
                    List<Cupom> cuponsASeremUtilizados = new List<Cupom>();
                    double valorTotalPedido = pedido.ValorTotalPedido;

                    valorTotalPedido -= pedido.CupomPromocional.Valor;
                    if (valorTotalPedido <= 0.00)
                    {
                        pedido.CuponsTroca = new List<Cupom>();
                        return null;
                    }

                    foreach (var item in pedido.CuponsTroca)
                    {
                        valorTotalPedido -= item.Valor;
                        if(valorTotalPedido >= 0)
                        {
                            cuponsASeremUtilizados.Add(item);
                        }
                    }
                    pedido.CuponsTroca = cuponsASeremUtilizados;

                    Cupom cupomTroca = new Cupom
                    {
                        UsuarioId = pedido.UsuarioId,
                        Tipo = 'T',
                        Valor = pedido.ValorTotalPedido * -1,
                        Usado = 0,
                        DataCadastro = DateTime.Now,
                        DataExpiracao = null
                    };
                    cupomTroca.Codigo = "T";
                    cupomTroca.Codigo += pedido.UsuarioId;
                    cupomTroca.Codigo += DateTime.Now.Year;
                    cupomTroca.Codigo += DateTime.Now.Month;
                    cupomTroca.Codigo += DateTime.Now.Day;
                    cupomTroca.Codigo += DateTime.Now.Hour;
                    cupomTroca.Codigo += DateTime.Now.Minute;

                    pedido.CupomTrocaGerado = cupomTroca;
                    pedido.ValorTotalPedido = 0;
                }
            }
            else
            {
                return "Deve ser registrado um pedido";
            }
            return null;
        }
    }
}
