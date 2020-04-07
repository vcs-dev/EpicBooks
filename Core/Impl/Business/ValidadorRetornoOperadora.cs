using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Impl.Business
{
    public class ValidadorRetornoOperadora : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                if (pedido.CartaoUm.Id < 1 && pedido.CartaoDois.Id < 1)
                    return null;
            }
            else
            {
                return "Deve ser registrado um Pedido.";
            }
            return null;
        }
    }
}
