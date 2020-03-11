using Core.Impl.DAO;
using Domain;
using Domain.Produto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Core.Impl.Produto.DAO
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

                SqlCommand comandolivro = new SqlCommand(cmdTextolivro, conexao, transacao);

                comandolivro.Parameters.AddWithValue("@Nome", livro.Nome);
                comandolivro.Parameters.AddWithValue("@Status", livro.Status);
                comandolivro.Parameters.AddWithValue("@CaminhoImagem", livro.CaminhoImagem);
                comandolivro.Parameters.AddWithValue("@Descricao", livro.Descricao);
                comandolivro.Parameters.AddWithValue("@Editora", livro.Editora);
                comandolivro.Parameters.AddWithValue("@AnoLancamento", livro.AnoLancamento);
                comandolivro.Parameters.AddWithValue("@Isbn", livro.Isbn);
                comandolivro.Parameters.AddWithValue("@DataCadastro", livro.DataCadastro);
                comandolivro.Parameters.AddWithValue("@QtdePaginas", livro.QtdePaginas);
                comandolivro.Parameters.AddWithValue("@Edicao", livro.Edicao);
                if (livro.Volume == null || livro.Volume == 0)
                    comandolivro.Parameters.AddWithValue("@Volume", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@Volume", livro.Volume);
                comandolivro.Parameters.AddWithValue("@Peso", livro.Peso);
                comandolivro.Parameters.AddWithValue("@Altura", livro.Altura);
                comandolivro.Parameters.AddWithValue("@Comprimento", livro.Comprimento);
                comandolivro.Parameters.AddWithValue("@Largura", livro.Largura);
                comandolivro.Parameters.AddWithValue("@TipoCapa", livro.TipoCapa);
                comandolivro.Parameters.AddWithValue("@GrupoPrecificacao", livro.GrupoPrecificacao);

                livro.Id = Convert.ToInt32(comandolivro.ExecuteScalar());

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

                Commit();
                comandolivro.Dispose();
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
            finally
            {
                Desconectar();
            }
        }
        public override void Alterar(EntidadeDominio entidade)
        {
            Livro livro = (Livro)entidade;
            string cmdTextolivro;
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

                SqlCommand comandolivro = new SqlCommand(cmdTextolivro, conexao, transacao);

                comandolivro.Parameters.AddWithValue("@ProdutoId", livro.Id);
                comandolivro.Parameters.AddWithValue("@Nome", livro.Nome);
                comandolivro.Parameters.AddWithValue("@Status", livro.Status);
                comandolivro.Parameters.AddWithValue("@CaminhoImagem", livro.CaminhoImagem);
                comandolivro.Parameters.AddWithValue("@Descricao", livro.Descricao);
                comandolivro.Parameters.AddWithValue("@Editora", livro.Editora);
                comandolivro.Parameters.AddWithValue("@AnoLancamento", livro.AnoLancamento);
                comandolivro.Parameters.AddWithValue("@Isbn", livro.Isbn);
                comandolivro.Parameters.AddWithValue("@QtdePaginas", livro.QtdePaginas);
                if(livro.Edicao == null)
                    comandolivro.Parameters.AddWithValue("@Edicao", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@Edicao", livro.Edicao);
                if(livro.Volume == null)
                    comandolivro.Parameters.AddWithValue("@Volume", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@Volume", livro.Volume);
                if (livro.Peso == null)
                    comandolivro.Parameters.AddWithValue("@Peso", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@Peso", livro.Peso);
                if (livro.Altura == null)
                    comandolivro.Parameters.AddWithValue("@Altura", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@Altura", livro.Altura);

                if (livro.Comprimento == null)
                    comandolivro.Parameters.AddWithValue("@Comprimento", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@Comprimento", livro.Comprimento);
                if (livro.Largura == null)
                    comandolivro.Parameters.AddWithValue("@Largura", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@Largura", livro.Largura);
                comandolivro.Parameters.AddWithValue("@TipoCapa", livro.TipoCapa);
                comandolivro.Parameters.AddWithValue("@GrupoPrecificacao", livro.GrupoPrecificacao);
                if (livro.MotivoMudancaStatus == null)
                    comandolivro.Parameters.AddWithValue("@MotivoMudancaStatus", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@MotivoMudancaStatus", livro.MotivoMudancaStatus);
                if (livro.CategoriaAtivacao == null)
                    comandolivro.Parameters.AddWithValue("@CategoriaAtivacao", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@CategoriaAtivacao", livro.CategoriaAtivacao);
                if (livro.CategoriaInativacao == null)
                    comandolivro.Parameters.AddWithValue("@CategoriaInativacao", DBNull.Value);
                else
                    comandolivro.Parameters.AddWithValue("@CategoriaInativacao", livro.CategoriaInativacao);

                comandolivro.ExecuteNonQuery();

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
                comandolivro.Dispose();
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
            //string precoVenda = "";
            //string cmdTextoEstoque;
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
                    cmdTextolivro = "SELECT * FROM Produtos WHERE Nome like '%@Nome%'";

                SqlCommand comandolivro = new SqlCommand(cmdTextolivro, conexao);

                if (livro.Id != 0)
                    comandolivro.Parameters.AddWithValue("@ProdutoId", livro.Id);
                if (!string.IsNullOrEmpty(livro.Isbn))
                    comandolivro.Parameters.AddWithValue("@Isbn", livro.Isbn);
                else if(!string.IsNullOrEmpty(livro.Nome))
                    comandolivro.Parameters.AddWithValue("@Nome", livro.Nome);

                SqlDataReader drlivro = comandolivro.ExecuteReader();
                comandolivro.Dispose();

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

                        //cmdTextoEstoque = "SELECT FORMAT(MAX(E.ValorCusto) * (1 + GP.PercentuaLucro), 'N', 'pt-br') AS PrecoVenda, " +
                        //                          "GP.PercentuaLucro " +
                        //                  "FROM EntradaEstoque E " +
                        //                  "JOIN Produtos P on(E.ProdutoId = P.ProdutoId) " +
                        //                  "JOIN GruposPrecificacao GP on(GP.GrupoPrecificacaoId = P.GrupoPrecificacao) " +
                        //                  "WHERE P.ProdutoId = @ProdutoId " +
                        //                  "GROUP BY GP.PercentuaLucro";

                        //SqlCommand comandoEstoque = new SqlCommand(cmdTextoEstoque, conexao);
                        //comandoEstoque.Parameters.AddWithValue("@ProdutoId", item.Id);
                        //SqlDataReader drValorCusto = comandoEstoque.ExecuteReader();
                        //comandoEstoque.Dispose();

                        //if (!drValorCusto.HasRows)
                        //{
                        //    throw new Exception("Sem Registros");
                        //}
                        //paisesProibicao = new List<int>();
                        //while (drValorCusto.Read())
                        //{
                        //    precoVenda = Convert.ToDouble(drValorCusto.GetString(0)).ToString("0.00");
                        //}
                        //drValorCusto.Close();
                        //item.PrecoVenda = precoVenda;
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
