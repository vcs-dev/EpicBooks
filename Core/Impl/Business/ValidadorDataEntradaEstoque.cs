using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;

namespace Core.Impl.Business
{
    public class ValidadorDataEntradaEstoque : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("EntradaEstoque"))
            {
                EntradaEstoque entradaEstoque = (EntradaEstoque)entidade;

                if (entradaEstoque.DataCadastro > DateTime.Now)
                    return "Datas futuras não são permitidas";
            }
            else
            {
                return "Deve ser registrada uma entrada de estoque.";
            }
            return null;
        }
    }
}
