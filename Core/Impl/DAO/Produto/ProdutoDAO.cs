using Core.Impl.DAO;
using Domain;
using Domain.Produto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Core.Impl.DAO.Produto
{
    public class ProdutoDAO : AbstractDAO
    {
        public ProdutoDAO() : base("Produtos", "ProdutoId")
        {
        }

        public override void Salvar(EntidadeDominio entidade)
        {
            Livro livro = (Livro)entidade;
            string cmdTextolivro;
            string cmdTextoGenero;
            string cmdTextoAutor;
            string cmdTextoEstoque;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextolivro = "INSERT INTO Produtos" +
                                    "(Nome," +
                                    "Status," +
                                    "CaminhoImagem," +
                                    "Descricao," +
                                    "Editora," +
                                    "AnoLancamento," +
                                    "Isbn," +
                                    "DataCadastro," +
                                    "QtdePaginas," +
                                    "Edicao," +
                                    "Volume," +
                                    "Peso," +
                                    "Altura," +
                                    "Comprimento," +
                                    "Largura," +
                                    "TipoCapa," +
                                    "GrupoPrecificacao" +
                                ") " +
                                "VALUES" +
                                    "(@Nome," +
                                    "@Status," +
                                    "@CaminhoImagem," +
                                    "@Descricao," +
                                    "@Editora," +
                                    "@AnoLancamento," +
                                    "@Isbn," +
                                    "@DataCadastro," +
                                    "@QtdePaginas," +
                                    "@Edicao," +
                                    "@Volume," +
                                    "@Peso," +
                                    "@Altura," +
                                    "@Comprimento," +
                                    "@Largura," +
                                    "@TipoCapa," +
                                    "@GrupoPrecificacao" +
                                ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoLivro = new SqlCommand(cmdTextolivro, conexao, transacao);

                comandoLivro.Parameters.AddWithValue("@Nome", livro.Nome);
                comandoLivro.Parameters.AddWithValue("@Status", livro.Status);
                comandoLivro.Parameters.AddWithValue("@CaminhoImagem", livro.CaminhoImagem);
                comandoLivro.Parameters.AddWithValue("@Descricao", livro.Descricao);
                comandoLivro.Parameters.AddWithValue("@Editora", livro.Editora);
                comandoLivro.Parameters.AddWithValue("@AnoLancamento", livro.AnoLancamento);
                comandoLivro.Parameters.AddWithValue("@Isbn", livro.Isbn);
                comandoLivro.Parameters.AddWithValue("@DataCadastro", livro.DataCadastro);
                comandoLivro.Parameters.AddWithValue("@QtdePaginas", livro.QtdePaginas);
                if (livro.Edicao == null)
                    comandoLivro.Parameters.AddWithValue("@Edicao", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Edicao", livro.Edicao);
                if (livro.Volume == null)
                    comandoLivro.Parameters.AddWithValue("@Volume", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Volume", livro.Volume);
                if (livro.Peso == null)
                    comandoLivro.Parameters.AddWithValue("@Peso", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Peso", livro.Peso);
                if (livro.Altura == null)
                    comandoLivro.Parameters.AddWithValue("@Altura", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Altura", livro.Altura);

                if (livro.Comprimento == null)
                    comandoLivro.Parameters.AddWithValue("@Comprimento", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Comprimento", livro.Comprimento);
                if (livro.Largura == null)
                    comandoLivro.Parameters.AddWithValue("@Largura", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Largura", livro.Largura);
                comandoLivro.Parameters.AddWithValue("@TipoCapa", livro.TipoCapa);
                comandoLivro.Parameters.AddWithValue("@GrupoPrecificacao", livro.GrupoPrecificacao);

                livro.Id = Convert.ToInt32(comandoLivro.ExecuteScalar());
                comandoLivro.Dispose();

                cmdTextoGenero = "INSERT INTO ProdutosGeneros" +
                                     "(ProdutoId," +
                                     "GeneroId" +
                                 ") " +
                                 "VALUES" +
                                     "(@ProdutoId," +
                                     "@GeneroId" +
                                 ")";

                SqlCommand comandoGenero = new SqlCommand(cmdTextoGenero, conexao, transacao);
                foreach (var item in livro.Generos)
                {
                    comandoGenero.Parameters.AddWithValue("@ProdutoId", livro.Id);
                    comandoGenero.Parameters.AddWithValue("@GeneroId", item);
                    comandoGenero.ExecuteNonQuery();
                    comandoGenero.Parameters.Clear();
                }
                comandoGenero.Dispose();

                cmdTextoAutor = "INSERT INTO ProdutosAutores" +
                                     "(ProdutoId," +
                                     "AutorId" +
                                 ") " +
                                 "VALUES" +
                                     "(@ProdutoId," +
                                     "@AutorId" +
                                 ")";

                SqlCommand comandoAutor = new SqlCommand(cmdTextoAutor, conexao, transacao);
                foreach (var item in livro.Autores)
                {
                    comandoAutor.Parameters.AddWithValue("@ProdutoId", livro.Id);
                    comandoAutor.Parameters.AddWithValue("@AutorId", item);
                    comandoAutor.ExecuteNonQuery();
                    comandoAutor.Parameters.Clear();
                }
                comandoAutor.Dispose();

                cmdTextoEstoque = "INSERT INTO Estoque" +
                                    "(ProdutoId," +
                                    "Qtde," +
                                    "ValorCusto" +
                                 ") " +
                                 "VALUES" +
                                     "(@ProdutoId," +
                                     "@Qtde," +
                                     "@ValorCusto" +
                                 ")";

                SqlCommand comandoEstoque = new SqlCommand(cmdTextoEstoque, conexao, transacao);

                comandoEstoque.Parameters.AddWithValue("@ProdutoId", livro.Id);
                comandoEstoque.Parameters.AddWithValue("@Qtde", 0);
                comandoEstoque.Parameters.AddWithValue("@ValorCusto", 0);
                comandoEstoque.ExecuteNonQuery();
                comandoEstoque.Dispose();

                Commit();
            }
            catch (SqlException e)
            {
                Rollback();
                throw e;
            }
            catch (InvalidOperationException e)
            {
                Rollback();
                throw e;
            }
            finally
            {
                Desconectar();
            }
        }
        public override void Alterar(EntidadeDominio entidade)
        {
            Livro livro = (Livro)entidade;
            string cmdTextolivro;
            string cmdTextoAutor;
            string cmdTextoGenero;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextolivro = "UPDATE Produtos SET " +
                                    "Nome = @Nome," +
                                    "Status = @Status," +
                                    "CaminhoImagem = @CaminhoImagem," +
                                    "Descricao = @Descricao," +
                                    "Editora = @Editora," +
                                    "AnoLancamento = @AnoLancamento," +
                                    "Isbn = @Isbn," +
                                    "QtdePaginas = @QtdePaginas," +
                                    "Edicao = @Edicao," +
                                    "Volume = @Volume," +
                                    "Peso = @Peso," +
                                    "Altura = @Altura," +
                                    "Comprimento = @Comprimento," +
                                    "Largura = @Largura," +
                                    "TipoCapa = @TipoCapa," +
                                    "GrupoPrecificacao = @GrupoPrecificacao," +
                                    "MotivoMudancaStatus = @MotivoMudancaStatus," +
                                    "CategoriaAtivacao = @CategoriaAtivacao," +
                                    "CategoriaInativacao = @CategoriaInativacao " +
                                "WHERE ProdutoId = @ProdutoId";

                SqlCommand comandoLivro = new SqlCommand(cmdTextolivro, conexao, transacao);

                comandoLivro.Parameters.AddWithValue("@ProdutoId", livro.Id);
                comandoLivro.Parameters.AddWithValue("@Nome", livro.Nome);
                comandoLivro.Parameters.AddWithValue("@Status", livro.Status);
                comandoLivro.Parameters.AddWithValue("@CaminhoImagem", livro.CaminhoImagem);
                comandoLivro.Parameters.AddWithValue("@Descricao", livro.Descricao);
                comandoLivro.Parameters.AddWithValue("@Editora", livro.Editora);
                comandoLivro.Parameters.AddWithValue("@AnoLancamento", livro.AnoLancamento);
                comandoLivro.Parameters.AddWithValue("@Isbn", livro.Isbn);
                comandoLivro.Parameters.AddWithValue("@QtdePaginas", livro.QtdePaginas);
                if(livro.Edicao == null)
                    comandoLivro.Parameters.AddWithValue("@Edicao", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Edicao", livro.Edicao);
                if(livro.Volume == null)
                    comandoLivro.Parameters.AddWithValue("@Volume", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Volume", livro.Volume);
                if (livro.Peso == null)
                    comandoLivro.Parameters.AddWithValue("@Peso", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Peso", livro.Peso);
                if (livro.Altura == null)
                    comandoLivro.Parameters.AddWithValue("@Altura", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Altura", livro.Altura);

                if (livro.Comprimento == null)
                    comandoLivro.Parameters.AddWithValue("@Comprimento", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Comprimento", livro.Comprimento);
                if (livro.Largura == null)
                    comandoLivro.Parameters.AddWithValue("@Largura", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@Largura", livro.Largura);
                comandoLivro.Parameters.AddWithValue("@TipoCapa", livro.TipoCapa);
                comandoLivro.Parameters.AddWithValue("@GrupoPrecificacao", livro.GrupoPrecificacao);
                if (livro.MotivoMudancaStatus == null)
                    comandoLivro.Parameters.AddWithValue("@MotivoMudancaStatus", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@MotivoMudancaStatus", livro.MotivoMudancaStatus);
                if (livro.CategoriaAtivacao == null)
                    comandoLivro.Parameters.AddWithValue("@CategoriaAtivacao", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@CategoriaAtivacao", livro.CategoriaAtivacao);
                if (livro.CategoriaInativacao == null)
                    comandoLivro.Parameters.AddWithValue("@CategoriaInativacao", DBNull.Value);
                else
                    comandoLivro.Parameters.AddWithValue("@CategoriaInativacao", livro.CategoriaInativacao);

                comandoLivro.ExecuteNonQuery();

                cmdTextoAutor = "DELETE FROM ProdutosAutores " +
                                 "WHERE ProdutoId = @ProdutoId";

                SqlCommand comandoAutor = new SqlCommand(cmdTextoAutor, conexao, transacao);
                comandoAutor.Parameters.AddWithValue("@ProdutoId", livro.Id);
                comandoAutor.ExecuteNonQuery();

                cmdTextoAutor = "INSERT INTO ProdutosAutores" +
                                    "(ProdutoId," +
                                    "AutorId" +
                                 ") " +
                                 "VALUES" +
                                     "(@ProdutoId," +
                                      "@AutorId" +
                                 ")";

                comandoAutor = new SqlCommand(cmdTextoAutor, conexao, transacao);

                foreach (var item in livro.Autores)
                {
                    comandoAutor.Parameters.AddWithValue("@ProdutoId", livro.Id);
                    comandoAutor.Parameters.AddWithValue("@AutorId", item);
                    comandoAutor.ExecuteNonQuery();
                    comandoAutor.Parameters.Clear();
                }

                cmdTextoGenero = "DELETE FROM ProdutosGeneros " +
                                 "WHERE ProdutoId = @ProdutoId";

                SqlCommand comandoGenero = new SqlCommand(cmdTextoGenero, conexao, transacao);
                comandoGenero.Parameters.AddWithValue("@ProdutoId", livro.Id);
                comandoGenero.ExecuteNonQuery();

                cmdTextoGenero = "INSERT INTO ProdutosGeneros" +
                                    "(ProdutoId," +
                                    "GeneroId" +
                                 ") " +
                                 "VALUES" +
                                     "(@ProdutoId," +
                                      "@GeneroId" +
                                 ")";

                comandoGenero = new SqlCommand(cmdTextoGenero, conexao, transacao);

                foreach (var item in livro.Generos)
                {
                    comandoGenero.Parameters.AddWithValue("@ProdutoId", livro.Id);
                    comandoGenero.Parameters.AddWithValue("@GeneroId", item);
                    comandoGenero.ExecuteNonQuery();
                    comandoGenero.Parameters.Clear();
                }

                Commit();
                comandoLivro.Dispose();
                comandoGenero.Dispose();
            }
            catch (SqlException e)
            {
                Rollback();
                throw e;
            }
            catch (InvalidOperationException e)
            {
                Rollback();
                throw e;
            }
            catch (InvalidCastException e)
            {
                Rollback();
                throw e;
            }
            catch (IOException e)
            {
                Rollback();
                throw e;
            }
            catch (Exception e)
            {
                Rollback();
                throw e;
            }
            finally
            {
                Desconectar();
            }
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Livro livro = (Livro)entidade;
            List<Livro> livros;
            List<int> generos;
            List<int> autores;
            string cmdTextolivro = "";
            string cmdTextoGenero;
            string cmdTextoAutor;
            double precoVenda = 0.0;
            string cmdTextoEstoque;
            try
            {
                Conectar();

                if (!string.IsNullOrEmpty(livro.Isbn) && livro.Id != 0)
                    cmdTextolivro = "SELECT * FROM Produtos WHERE ProdutoId = @ProdutoId AND Isbn = @Isbn";
                else if (!string.IsNullOrEmpty(livro.Isbn))
                    cmdTextolivro = "SELECT * FROM Produtos WHERE Isbn = @Isbn";
                else if (livro.Id == 0 && string.IsNullOrEmpty(livro.Nome))
                    cmdTextolivro = "SELECT * FROM Produtos";
                else if (livro.Id != 0 && string.IsNullOrEmpty(livro.Nome))
                    cmdTextolivro = "SELECT * FROM Produtos WHERE ProdutoId = @ProdutoId";
                else if (livro.Id == 0 && !string.IsNullOrEmpty(livro.Nome))
                    cmdTextolivro = "SELECT * FROM Produtos WHERE Nome LIKE @Nome";

                SqlCommand comandoLivro = new SqlCommand(cmdTextolivro, conexao);

                if (livro.Id != 0)
                    comandoLivro.Parameters.AddWithValue("@ProdutoId", livro.Id);
                if (!string.IsNullOrEmpty(livro.Isbn))
                    comandoLivro.Parameters.AddWithValue("@Isbn", livro.Isbn);
                else if(!string.IsNullOrEmpty(livro.Nome))
                    comandoLivro.Parameters.AddWithValue("@Nome", "%" + livro.Nome + "%");

                SqlDataReader drlivro = comandoLivro.ExecuteReader();
                comandoLivro.Dispose();

                livros = DataReaderlivroParaList(drlivro);

                if (livros.Count > 0)
                {
                    foreach (var item in livros)
                    {
                        cmdTextoGenero = "SELECT G.GeneroId " +
                                         "FROM Produtos P " +
                                             "JOIN ProdutosGeneros PG " +
                                             "ON(P.ProdutoId = PG.ProdutoId) " +
                                             "JOIN Generos G " +
                                             "ON(PG.GeneroId = G.GeneroId)" +
                                         "WHERE P.ProdutoId = @ProdutoId";

                        SqlCommand comandoGenero = new SqlCommand(cmdTextoGenero, conexao);
                        comandoGenero.Parameters.AddWithValue("@ProdutoId", item.Id);
                        SqlDataReader drGenero = comandoGenero.ExecuteReader();
                        comandoGenero.Dispose();

                        if (!drGenero.HasRows)
                        {
                            throw new Exception("Sem Registros");
                        }
                        generos = new List<int>();
                        while (drGenero.Read())
                        {
                            generos.Add(drGenero.GetInt32(0));
                        }
                        drGenero.Close();
                        item.Generos = generos;
                    }

                    foreach (var item in livros)
                    {
                        cmdTextoAutor = "SELECT A.AutorId " +
                                         "FROM Produtos P " +
                                             "JOIN ProdutosAutores PA " +
                                             "ON(P.ProdutoId = PA.ProdutoId) " +
                                             "JOIN Autores A " +
                                             "ON(PA.AutorId = A.AutorId)" +
                                         "WHERE P.ProdutoId = @ProdutoId";

                        SqlCommand comandoAutor = new SqlCommand(cmdTextoAutor, conexao);
                        comandoAutor.Parameters.AddWithValue("@ProdutoId", item.Id);
                        SqlDataReader drAutor = comandoAutor.ExecuteReader();
                        comandoAutor.Dispose();

                        if (!drAutor.HasRows)
                        {
                            throw new Exception("Sem Registros");
                        }
                        autores = new List<int>();
                        while (drAutor.Read())
                        {
                            autores.Add(drAutor.GetInt32(0));
                        }
                        drAutor.Close();
                        item.Autores = autores;

                        cmdTextoEstoque = "SELECT ROUND(E.ValorCusto * (1 + GP.PercentualLucro), 2) AS PrecoVenda " +
                                          "FROM Estoque E " +
                                              "JOIN Produtos P on(E.ProdutoId = P.ProdutoId) " +
                                              "JOIN GruposPrecificacao GP on(GP.GrupoId = P.GrupoPrecificacao) " +
                                          "WHERE P.ProdutoId = @ProdutoId";
                        SqlCommand comandoEstoque = new SqlCommand(cmdTextoEstoque, conexao);
                        comandoEstoque.Parameters.AddWithValue("@ProdutoId", item.Id);
                        SqlDataReader drValorCusto = comandoEstoque.ExecuteReader();
                        comandoEstoque.Dispose();

                        if (!drValorCusto.HasRows)
                        {
                            throw new Exception("Sem Registros");
                        }
                        while (drValorCusto.Read())
                        {
                            precoVenda = Convert.ToDouble(drValorCusto.GetDecimal(0));
                        }
                        drValorCusto.Close();
                        item.PrecoVenda = precoVenda;
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
            finally
            {
                Desconectar();
            }
            return livros.ToList<EntidadeDominio>();
        }
        public List<Livro> DataReaderlivroParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
            {
                throw new Exception("Sem Registros");
            }

            List<Livro> livros = new List<Livro>();
            while (dataReader.Read())
            {
                try
                {
                    Livro livro = new Livro
                    {
                        Id = Convert.ToInt32(dataReader["ProdutoId"]),
                        Nome = dataReader["Nome"].ToString(),
                        Status = Convert.ToByte(dataReader["Status"]),
                        CaminhoImagem = dataReader["CaminhoImagem"].ToString(),
                        Descricao = dataReader["Descricao"].ToString(),
                        Editora = Convert.ToInt32(dataReader["Editora"]),
                        AnoLancamento = dataReader["AnoLancamento"].ToString(),
                        Isbn = dataReader["Isbn"].ToString(),
                        DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                        QtdePaginas = Convert.ToInt32(dataReader["QtdePaginas"]),
                        Edicao = Convert.ToInt32(dataReader["Edicao"]),
                        TipoCapa = Convert.ToInt32(dataReader["TipoCapa"]),
                        GrupoPrecificacao = Convert.ToInt32(dataReader["GrupoPrecificacao"]),
                    };
                    if (!dataReader.IsDBNull(11))
                        livro.Volume = Convert.ToInt32(dataReader["Volume"]);
                    if (!dataReader.IsDBNull(12))
                        livro.Peso = Convert.ToInt32(dataReader["Peso"]);
                    if (!dataReader.IsDBNull(13))
                        livro.Altura = Convert.ToDecimal(dataReader["Altura"]);
                    if (!dataReader.IsDBNull(14))
                        livro.Comprimento = Convert.ToDecimal(dataReader["Comprimento"]);
                    if (!dataReader.IsDBNull(15))
                        livro.Largura = Convert.ToDecimal(dataReader["Largura"]);
                    if (!dataReader.IsDBNull(18))
                        livro.MotivoMudancaStatus = dataReader["MotivoMudancaStatus"].ToString();
                    if (!dataReader.IsDBNull(19))
                        livro.CategoriaAtivacao = Convert.ToInt32(dataReader["CategoriaAtivacao"]);
                    if (!dataReader.IsDBNull(20))
                        livro.CategoriaInativacao = Convert.ToInt32(dataReader["CategoriaInativacao"]);

                    livros.Add(livro);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return livros.ToList();
        }
    }
}
