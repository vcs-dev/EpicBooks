using Core.Interfaces;
using Domain;
using Domain.Produto;
using System;

namespace Core.Impl.Business
{
    public class ValidadorIsbn : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Livro"))
            {
                Livro livro = (Livro)entidade;
                if ((livro.Isbn.Length != 10 && livro.Isbn.Length != 13) || Int32.TryParse(livro.Isbn, out _))
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
