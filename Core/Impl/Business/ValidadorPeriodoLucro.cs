using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;

namespace Core.Impl.Business
{
    public class ValidadorPeriodoLucro : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Lucro"))
            {
                Lucro lucro = (Lucro)entidade;

                if (string.IsNullOrEmpty(lucro.DataInicial) || string.IsNullOrEmpty(lucro.DataFinal) ||
                    string.IsNullOrWhiteSpace(lucro.DataInicial) || string.IsNullOrWhiteSpace(lucro.DataFinal))
                    return "Informe um período válido.";
                if (lucro.DataInicial.ToString().Length < 10 || !lucro.DataInicial.ToString().Contains('/'))
                    return "Data inicial em formato incorreto.";
                if (lucro.DataFinal.ToString().Length < 10 || !lucro.DataFinal.ToString().Contains('/'))
                    return "Data final em formato incorreto.";
                if (lucro.DataInicial.Equals(lucro.DataFinal))
                    return "A data inicial não pode ser igual à data final.";
                if (Convert.ToDateTime(lucro.DataInicial).AddDays(30) > Convert.ToDateTime(lucro.DataFinal))
                    return "Informe um período igual ou superior a 30 dias";
                if (Convert.ToDateTime(lucro.DataInicial) > Convert.ToDateTime(lucro.DataFinal))
                    return "A data inicial não pode ser maior que a data final.";
            }
            else
            {
                return "Deve ser consultado um lucro.";
            }
            return null;
        }
    }
}
