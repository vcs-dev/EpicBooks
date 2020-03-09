using Core.Interfaces;
using Domain;
using Domain.Produto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Impl.Business
{
    public class ValidadorIsbn : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Livro"))
            {
                Livro livro = (Livro)entidade;
                if ((livro.Isbn.Length != 10 && livro.Isbn.Length != 13) || livro.Isbn.Contains('-') || !Int32.TryParse(livro.Isbn, out _))
                    return "Campo ISBN preenchido em formato incorreto.";
            }
            else
            {
                return "Deve ser registrado um livro.";
            }
            return null;
        }
    }
}
