using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;

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

                    valorTotalPedido += pedido.CupomPromocional.Valor;
                    if (valorTotalPedido >= 0.00)
                    {
                        pedido.CupomPromocional = new Cupom();
                        return "Cupom promocional desnecessário. Os cupons de troca selecionados cobrem a compra.";
                    }

                    pedido.CuponsTroca = pedido.CuponsTroca.OrderByDescending(x => x.Valor).ToList();                   

                    foreach (var item in pedido.CuponsTroca)
                    {
                        valorTotalPedido += item.Valor;
                        if(valorTotalPedido >= 0)
                        {
                            cuponsASeremUtilizados.Add(item);
                            break;
                        }
                    }
                    pedido.CuponsTroca = cuponsASeremUtilizados;

                    Cupom cupomTroca = new Cupom
                    {
                        UsuarioId = pedido.UsuarioId,
                        Tipo = 'T',
                        Valor = Math.Round(pedido.ValorTotalPedido * -1, 2),
                        Usado = 0,
                        DataCadastro = DateTime.Now,
                        DataExpiracao = null
                    };
                    cupomTroca.Codigo = "T";
                    cupomTroca.Codigo += pedido.UsuarioId;
                    cupomTroca.Codigo += DateTime.Now.Year;
                    cupomTroca.Codigo += DateTime.Now.Month;
                    cupomTroca.Codigo += DateTime.Now.Day;

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
