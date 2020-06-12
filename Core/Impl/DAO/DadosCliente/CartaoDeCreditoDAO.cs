using Domain;
using Domain.DadosCliente;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.DadosCliente
{
    public class CartaoDeCreditoDAO :AbstractDAO
    {
        public CartaoDeCreditoDAO():base("CartoesDeCredito", "CartaoId")
        {
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            CartaoDeCredito cartao = (CartaoDeCredito)entidade;
            List<CartaoDeCredito> cartoes;
            string cmdTextoCartao;

            try
            {
                Conectar();

                if (cartao.Id > 0)
                    cmdTextoCartao = "SELECT * FROM CartoesDeCredito WHERE CartaoId = @CartaoId AND Ativo = 1";
                else if (cartao.UsuarioId > 0)
                    cmdTextoCartao = "SELECT * " +
                                     "FROM CartoesDeCredito CC " +
                                        "JOIN UsuariosCartoes UC ON(CC.CartaoId = UC.CartaoId) " +
                                     "WHERE UsuarioId = @UsuarioId AND Ativo = 1";
                else
                    cmdTextoCartao = "SELECT * FROM CartoesDeCredito";

                SqlCommand comandoCartao = new SqlCommand(cmdTextoCartao, conexao);

                if (cartao.Id > 0)
                    comandoCartao.Parameters.AddWithValue("@CartaoId", cartao.Id);
                else if (cartao.UsuarioId > 0)
                    comandoCartao.Parameters.AddWithValue("@UsuarioId", cartao.UsuarioId);

                SqlDataReader drCartao = comandoCartao.ExecuteReader();
                comandoCartao.Dispose();

                cartoes = DataReaderCartaoParaList(drCartao);
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
            return cartoes.ToList<EntidadeDominio>();
        }

        public List<CartaoDeCredito> DataReaderCartaoParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<CartaoDeCredito>();

            List<CartaoDeCredito> cartoes = new List<CartaoDeCredito>();
            while (dataReader.Read())
            {
                try
                {
                    CartaoDeCredito cartao = new CartaoDeCredito
                    {
                        Id = Convert.ToInt32(dataReader["CartaoId"]),
                        Bandeira = Convert.ToInt32(dataReader["Bandeira"]),
                        Numeracao = dataReader["Numeracao"].ToString(),
                        NomeImpresso = dataReader["NomeImpresso"].ToString(),
                        Validade = dataReader["Validade"].ToString(),
                        Apelido = dataReader["Apelido"].ToString()
                    };

                    cartoes.Add(cartao);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return cartoes;
        }
    }
}
