using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;

namespace Core.Impl.Business
{
    public class ValidadorPeriodoVenda : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Venda"))
            {
                Venda venda = (Venda)entidade;

                if (string.IsNullOrEmpty(venda.DataInicial) || string.IsNullOrEmpty(venda.DataFinal) ||
                    string.IsNullOrWhiteSpace(venda.DataInicial) || string.IsNullOrWhiteSpace(venda.DataFinal))
                    return "Informe um período válido.";
                if (venda.DataInicial.ToString().Length < 10 || !venda.DataInicial.ToString().Contains('/'))
                    return "Data inicial em formato incorreto.";
                if (venda.DataFinal.ToString().Length < 10 || !venda.DataFinal.ToString().Contains('/'))
                    return "Data final em formato incorreto.";
                if (venda.DataInicial.Equals(venda.DataFinal))
                    return "A data inicial não pode ser igual à data final.";
                if (Convert.ToDateTime(venda.DataInicial).AddDays(30) > Convert.ToDateTime(venda.DataFinal))
                    return "Informe um período igual ou superior a 30 dias";
                if (Convert.ToDateTime(venda.DataInicial) > Convert.ToDateTime(venda.DataFinal))
                    return "A data inicial não pode ser maior que a data final.";
            }
            else
            {
                return "Deve ser consultado um venda.";
            }
            return null;
        }
    }
}
