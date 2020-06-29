using Core.Application;
using Core.Impl.Control;
using Core.Interfaces;
using Domain;
using Domain.Produto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Impl.Business
{
    public class ValidadorDadosObrigatoriosProdutoEdicao : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Livro"))
            {
                Livro livro = (Livro)entidade;
                List<Livro> livros;
                Result resultado;

                if (string.IsNullOrEmpty(livro.Nome) || string.IsNullOrWhiteSpace(livro.Nome) ||
                    string.IsNullOrEmpty(livro.Descricao) || string.IsNullOrWhiteSpace(livro.Descricao) ||
                    livro.Generos.Count() == 0 || livro.Autores.Count() == 0 ||
                    livro.Editora == null || livro.Editora == 0 || livro.GrupoPrecificacao == 0 ||
                    string.IsNullOrEmpty(livro.AnoLancamento) || string.IsNullOrWhiteSpace(livro.AnoLancamento) ||
                    livro.Edicao == null || livro.Editora == 0 ||
                    string.IsNullOrEmpty(livro.CaminhoImagem) || string.IsNullOrWhiteSpace(livro.CaminhoImagem) ||
                    string.IsNullOrEmpty(livro.Isbn) || string.IsNullOrWhiteSpace(livro.Isbn) ||
                    livro.QtdePaginas == null || livro.QtdePaginas == 0 || livro.TipoCapa == 0)
                    return "Os campos com * são de preenchimento obrigatório.";

                resultado = new Facade().Consultar(new Livro { Id = livro.Id });
                livros = new List<Livro>();
                if (resultado.Msg != null)
                    return "Erro ao validar edição do livro";
                else
                {
                    foreach (var item in resultado.Entidades)
                    {
                        livros.Add((Livro)item);
                    }
                }
                if (livros.FirstOrDefault().Status != livro.Status)
                {
                    if(string.IsNullOrEmpty(livro.MotivoMudancaStatus) || string.IsNullOrWhiteSpace(livro.MotivoMudancaStatus))
                        return "Os campos com * são de preenchimento obrigatório.";
                    if (livros.FirstOrDefault().MotivoMudancaStatus.Equals(livro.MotivoMudancaStatus))
                        return "O motivo da mudança de status deve ser diferente do anterior.";
                    if (livro.Status == 1)
                    {
                        if(livro.CategoriaAtivacao == null || livro.CategoriaAtivacao < 1)
                            return "Os campos com * são de preenchimento obrigatório.";
                    }
                    else if (livro.Status == 0)
                    {
                        if (livro.CategoriaInativacao == null || livro.CategoriaInativacao < 1)
                            return "Os campos com * são de preenchimento obrigatório.";
                    }
                }
                else
                {
                    if (livro.Status == 1)
                    {
                        if (livro.CategoriaAtivacao == null || livro.CategoriaAtivacao < 1)
                            return "Os campos com * são de preenchimento obrigatório.";
                    }
                    else if (livro.Status == 0)
                    {
                        if (livro.CategoriaInativacao == null || livro.CategoriaInativacao < 1)
                            return "Os campos com * são de preenchimento obrigatório.";
                    }
                }
            }
            else
            {
                return "Deve ser registrado um livro.";
            }
            return null;
        }
    }
}
