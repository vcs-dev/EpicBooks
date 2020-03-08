using Core.Interfaces;
using Domain;
using System;

namespace Core.Impl.Business
{
    public class InclusaoDataCadastro : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade != null)
            {
                entidade.DataCadastro = DateTime.Now;
            }
            else
            {
                return "Entidade: " + entidade.GetType().Name + " nula!";
            }
            return null;
        }
    }
}
