using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;

namespace Core.Impl.Business
{
    public class ValidadorPeriodoFaturamento : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Faturamento"))
            {
                Faturamento faturamento = (Faturamento)entidade;

                if (string.IsNullOrEmpty(faturamento.DataInicial) || string.IsNullOrEmpty(faturamento.DataFinal) ||
                    string.IsNullOrWhiteSpace(faturamento.DataInicial) || string.IsNullOrWhiteSpace(faturamento.DataFinal))
                    return "Informe um período válido.";
                if (faturamento.DataInicial.ToString().Length < 10 || !faturamento.DataInicial.ToString().Contains('/'))
                    return "Data inicial em formato incorreto.";
                if (faturamento.DataFinal.ToString().Length < 10 || !faturamento.DataFinal.ToString().Contains('/'))
                    return "Data final em formato incorreto.";
                if (faturamento.DataInicial.Equals(faturamento.DataFinal))
                    return "A data inicial não pode ser igual à data final.";
                if (Convert.ToDateTime(faturamento.DataInicial).AddDays(30) > Convert.ToDateTime(faturamento.DataFinal))
                    return "Informe um período igual ou superior a 30 dias";
                if (Convert.ToDateTime(faturamento.DataInicial) > Convert.ToDateTime(faturamento.DataFinal))
                    return "A data inicial não pode ser maior que a data final.";
            }
            else
            {
                return "Deve ser consultado um faturamento.";
            }
            return null;
        }
    }
}
