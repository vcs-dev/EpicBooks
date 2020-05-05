using Core.Application;
using Core.Impl.Control;
using Core.Impl.DAO;
using Core.Interfaces;
using Domain;
using Domain.DadosCliente;
using Domain.Negocio;
using System.Collections.Generic;

namespace Core.Impl.Business
{
    public class ValidadorRetornoOperadora : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Pedido"))
            {
                Pedido pedido = (Pedido)entidade;
                Result resultado;
                if (pedido.CartaoUm.Id > 0 && !pedido.IsAlteracaoGerencial)
                {
                    resultado = new Facade().Consultar(new CartaoDeCredito { UsuarioId = pedido.UsuarioId });
                    if (resultado.Msg != null)
                    {
                        pedido.Status = 'R';
                        return "Não foi possível validar o cartão.\n" + resultado.Msg;
                    }
                    List<CartaoDeCredito> cartoes = new List<CartaoDeCredito>();
                    foreach (var item in resultado.Entidades)
                    {
                        cartoes.Add((CartaoDeCredito)item);
                    }

                    CartaoDeCredito temp = cartoes.Find(x => x.Id == pedido.CartaoUm.Id);

                    if (temp != null)
                        pedido.CartaoUm.Numeracao = temp.Numeracao;
                    else
                        return "Cartão não encontrado na base";

                    if (pedido.CartaoDois.Id > 0)
                    {
                        temp = null;

                        temp = cartoes.Find(x => x.Id == pedido.CartaoDois.Id);
                        if (temp != null)
                            pedido.CartaoDois.Numeracao = temp.Numeracao;
                        else
                            return "Cartão não encontrado na base";
                    }
                    OperadoraCartaoDAO operadoraCartaoDAO = new OperadoraCartaoDAO();

                    resultado.Entidades = operadoraCartaoDAO.Consultar(pedido.CartaoUm);
                    if (resultado.Entidades.Count > 0)
                    {
                        pedido.Status = 'R';
                        return null;
                    }

                    if (pedido.CartaoDois.Id > 0)
                    {
                        resultado.Entidades = operadoraCartaoDAO.Consultar(pedido.CartaoDois);
                        if (resultado.Entidades.Count > 0)
                        {
                            pedido.Status = 'R';
                            return null;
                        }
                    }
                }
                if(!pedido.IsAlteracaoGerencial)
                    pedido.Status = 'A';
            }
            else
            {
                return "Deve ser registrado um Pedido.";
            }
            return null;
        }
    }
}
