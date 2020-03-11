using Core.Interfaces;
using Domain;
using Domain.Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosProdutoEdicao : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Livro"))
            {
                Livro livro = (Livro)entidade;

                if (string.IsNullOrEmpty(livro.Nome.Trim()) || string.IsNullOrEmpty(livro.Descricao.Trim()) ||
                    livro.Generos.Count() == 0 || livro.Autores.Count() == 0 ||
                    livro.Editora == null || livro.Editora == 0 || livro.GrupoPrecificacao == 0 ||
                    string.IsNullOrEmpty(livro.AnoLancamento.Trim()) || livro.Edicao == null || livro.Editora == 0 ||
                    string.IsNullOrEmpty(livro.CaminhoImagem.Trim()) || string.IsNullOrEmpty(livro.Isbn.Trim()) ||
                    livro.QtdePaginas == null || livro.QtdePaginas == 0 || livro.TipoCapa == 0)
                    return "Os campos com * são de preenchimento obrigatório.";
                if (livro.Status == 0 &&
                    (string.IsNullOrEmpty(livro.MotivoMudancaStatus.Trim()) || livro.CategoriaInativacao == null || livro.CategoriaInativacao == 0))
                    return "Os campos com * são de preenchimento obrigatório.";
                else if (livro.Status == 1 &&
                    (string.IsNullOrEmpty(livro.MotivoMudancaStatus.Trim()) || livro.CategoriaAtivacao == null || livro.CategoriaAtivacao == 0))
                    return "Os campos com * são de preenchimento obrigatório.";
            }
            else
            {
                return "Deve ser registrado um livro.";
            }
            return null;
        }
    }
}
