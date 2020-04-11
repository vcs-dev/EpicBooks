using Domain;
using Domain.DadosCliente;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO
{
    public class OperadoraCartaoDAO : AbstractDAO
    {
        public OperadoraCartaoDAO():base("ListaNegraCartoes", "Id")
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

                cmdTextoCartao = "SELECT * FROM ListaNegraCartoes WHERE Numeracao = @Numeracao";

                SqlCommand comandoCartao = new SqlCommand(cmdTextoCartao, conexao);

                comandoCartao.Parameters.AddWithValue("@Numeracao", cartao.Numeracao);

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
                        Numeracao = dataReader["Numeracao"].ToString()
                    };

                    cartoes.Add(cartao);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return cartoes.ToList();
        }
    }
}
