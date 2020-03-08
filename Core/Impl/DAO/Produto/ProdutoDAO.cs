using Core.Impl.DAO;
using Domain;
using Domain.Produto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.Produto.DAO
{
    public class ProdutoDAO : AbstractDAO
    {
        public ProdutoDAO() : base("Livros", "LivroId")
        {
        }
    }
}
