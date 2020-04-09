using Core.Application;
using Core.Impl.Control;
using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System.Collections.Generic;


namespace Core.Impl.Business
{
    public class ValidadorCupons : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;

                Result resultado;
                List<Cupom> cupons;

                resultado = new Facade().Consultar(new Cupom { UsuarioId = pedido.UsuarioId });
                if (resultado.Msg != null)
                    return "Não foi possível validar os cupons\n" + resultado.Msg;

                cupons = new List<Cupom>();
                foreach (var item in resultado.Entidades)
                {
                    cupons.Add((Cupom)item);
                }

                Cupom temp = null;
                foreach (var item in pedido.CuponsTroca)
                {
                    temp = cupons.Find(x => x.Id == item.Id);
                    if (temp == null)
                        return "Cupom " + item.Codigo + "inválido";
                }
            }
            else
            {
                return "Deve ser registrado um Pedido.";
            }
            return null;
        }
    }
}
