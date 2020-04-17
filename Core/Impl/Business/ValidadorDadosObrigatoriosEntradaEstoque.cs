using Core.Interfaces;
using Domain;
using Domain.Negocio;
using System;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosEntradaEstoque : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("EntradaEstoque"))
            {
                EntradaEstoque entradaEstoque = (EntradaEstoque)entidade;

                if (entradaEstoque.FornecedorId <= 0 || entradaEstoque.Qtde <= 0 || entradaEstoque.ValorCusto <= 0 ||
                    entradaEstoque.DataCadastro == DateTime.MinValue)
                    return "Os campos com * são de preenchimento obrigatório.";
            }
            else
            {
                return "Deve ser registrada uma entrada de estoque.";
            }
            return null;
        }
    }
}
