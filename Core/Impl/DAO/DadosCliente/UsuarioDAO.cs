using Domain;
using Domain.DadosCliente;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Impl.DAO.DadosCliente
{
    public class UsuarioDAO : AbstractDAO
    {
        public UsuarioDAO() : base("Usuarios", "UsuarioId")
        {
        }
        public override void Salvar(EntidadeDominio entidade)
        {
            Usuario usuario = (Usuario)entidade;
            List<int> idsEnderecos;
            string cmdTextoUsuario;
            string cmdTextoCartao;
            string cmdTextoEndereco;

            try
            {
                Conectar();
                BeginTransaction();

                cmdTextoCartao = "INSERT INTO CartoesDeCredito(Bandeira," +
                                                              "Numeracao," +
                                                              "NomeImpresso, " +
                                                              "Validade" +
                                 ") " +
                                 "VALUES(@Bandeira," +
                                        "@Numeracao," +
                                        "@NomeImpresso, " +
                                        "@Validade" +
                                 ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoCartao = new SqlCommand(cmdTextoCartao, conexao, transacao);

                comandoCartao.Parameters.AddWithValue("@Bandeira", usuario.Cartao.Bandeira);
                comandoCartao.Parameters.AddWithValue("@Numeracao", usuario.Cartao.Numeracao);
                comandoCartao.Parameters.AddWithValue("@NomeImpresso", usuario.Cartao.NomeImpresso);
                comandoCartao.Parameters.AddWithValue("@Validade", usuario.Cartao.Validade);
                usuario.Cartao.Id = Convert.ToByte(comandoCartao.ExecuteScalar());

                cmdTextoUsuario = "INSERT INTO Usuarios(NomeCompleto," +
                                                       "Sexo," +
                                                       "DataNascimento," +
                                                       "Cpf," +
                                                       "TelefoneTipo," +
                                                       "TelefoneDdd," +
                                                       "TelefoneNumero," +
                                                       "Email," +
                                                       "Senha," +
                                                       "DataCadastro," +
                                                       "CartaoPreferencial" +
                                  ") " +
                                  "VALUES(@NomeCompleto," +
                                         "@Sexo," +
                                         "@DataNascimento," +
                                         "@Cpf," +
                                         "@TelefoneTipo," +
                                         "@TelefoneDdd," +
                                         "@TelefoneNumero," +
                                         "@Email," +
                                         "CONVERT(varchar(32),HASHBYTES('SHA2_256', @Senha),1)," +
                                         "@DataCadastro, " +
                                         "@CartaoPreferencial" +
                                  ") SELECT CAST(scope_identity() AS int)";

                SqlCommand comandoUsuario = new SqlCommand(cmdTextoUsuario, conexao, transacao);

                comandoUsuario.Parameters.AddWithValue("@NomeCompleto", usuario.NomeCompleto);
                comandoUsuario.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                comandoUsuario.Parameters.AddWithValue("@DataNascimento", usuario.DataNascimento);
                comandoUsuario.Parameters.AddWithValue("@Cpf", usuario.Cpf);
                comandoUsuario.Parameters.AddWithValue("@TelefoneTipo", usuario.TelefoneTipo);
                comandoUsuario.Parameters.AddWithValue("@TelefoneDdd", usuario.TelefoneDdd);
                comandoUsuario.Parameters.AddWithValue("@TelefoneNumero", usuario.TelefoneNumero);
                comandoUsuario.Parameters.AddWithValue("@Email", usuario.Email);
                comandoUsuario.Parameters.AddWithValue("@Senha", usuario.Senha);
                comandoUsuario.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);
                comandoUsuario.Parameters.AddWithValue("@CartaoPreferencial", usuario.Cartao.Id);
                usuario.Id = Convert.ToInt32(comandoUsuario.ExecuteScalar());
                comandoUsuario.Dispose();

                cmdTextoCartao = "INSERT INTO UsuariosCartoes(UsuarioId," +
                                             "CartaoId" +
                 ") " +
                 "VALUES(@UsuarioId," +
                        "@CartaoId" +
                 ")";
                comandoCartao = new SqlCommand(cmdTextoCartao, conexao, transacao);

                comandoCartao.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                comandoCartao.Parameters.AddWithValue("@CartaoId", usuario.Cartao.Id);
                comandoCartao.ExecuteNonQuery();
                comandoCartao.Dispose();

                cmdTextoEndereco = "INSERT INTO Enderecos(TipoEndereco," +
                                                         "TipoResidencia, " +
                                                         "TipoLogradouro, " +
                                                         "Logradouro, " +
                                                         "Cep, " +
                                                         "Numero, " +
                                                         "Bairro, " +
                                                         "Cidade, " +
                                                         "Estado, " +
                                                         "Pais, " +
                                                         "Observacao" +
                                   ") " +
                                   "VALUES(@TipoEndereco," +
                                          "@TipoResidencia, " +
                                          "@TipoLogradouro, " +
                                          "@Logradouro, " +
                                          "@Cep, " +
                                          "@Numero, " +
                                          "@Bairro, " +
                                          "@Cidade, " +
                                          "@Estado, " +
                                          "@Pais, " +
                                          "@Observacao" +
                                   ") SELECT CAST(scope_identity() AS int)";
                SqlCommand comandoEndereco = new SqlCommand(cmdTextoEndereco, conexao, transacao);

                idsEnderecos = new List<int>();

                comandoEndereco.Parameters.AddWithValue("@TipoEndereco", usuario.EnderecoEntrega.TipoEndereco);
                comandoEndereco.Parameters.AddWithValue("@TipoResidencia", usuario.EnderecoEntrega.TipoResidencia);
                comandoEndereco.Parameters.AddWithValue("@TipoLogradouro", usuario.EnderecoEntrega.TipoLogradouro);
                comandoEndereco.Parameters.AddWithValue("@Logradouro", usuario.EnderecoEntrega.Logradouro);
                comandoEndereco.Parameters.AddWithValue("@Cep", usuario.EnderecoEntrega.Cep);
                comandoEndereco.Parameters.AddWithValue("@Numero", usuario.EnderecoEntrega.Numero);
                comandoEndereco.Parameters.AddWithValue("@Bairro", usuario.EnderecoEntrega.Bairro);
                comandoEndereco.Parameters.AddWithValue("@Cidade", usuario.EnderecoEntrega.Cidade);
                comandoEndereco.Parameters.AddWithValue("@Estado", usuario.EnderecoEntrega.Estado);
                comandoEndereco.Parameters.AddWithValue("@Pais", usuario.EnderecoEntrega.Pais);
                if (string.IsNullOrEmpty(usuario.EnderecoEntrega.Observacao))
                    comandoEndereco.Parameters.AddWithValue("@Observacao", DBNull.Value);
                else
                    comandoEndereco.Parameters.AddWithValue("@Observacao", usuario.EnderecoEntrega.Observacao);
                idsEnderecos.Add(Convert.ToInt32(comandoEndereco.ExecuteScalar()));
                comandoEndereco.Parameters.Clear();

                if (usuario.EndEntregaECobranca == 1)
                {
                    comandoEndereco.Parameters.AddWithValue("@TipoEndereco", 2);//Tipo 2: Cobranca
                    comandoEndereco.Parameters.AddWithValue("@TipoResidencia", usuario.EnderecoEntrega.TipoResidencia);
                    comandoEndereco.Parameters.AddWithValue("@TipoLogradouro", usuario.EnderecoEntrega.TipoLogradouro);
                    comandoEndereco.Parameters.AddWithValue("@Logradouro", usuario.EnderecoEntrega.Logradouro);
                    comandoEndereco.Parameters.AddWithValue("@Cep", usuario.EnderecoEntrega.Cep);
                    comandoEndereco.Parameters.AddWithValue("@Numero", usuario.EnderecoEntrega.Numero);
                    comandoEndereco.Parameters.AddWithValue("@Bairro", usuario.EnderecoEntrega.Bairro);
                    comandoEndereco.Parameters.AddWithValue("@Cidade", usuario.EnderecoEntrega.Cidade);
                    comandoEndereco.Parameters.AddWithValue("@Estado", usuario.EnderecoEntrega.Estado);
                    comandoEndereco.Parameters.AddWithValue("@Pais", usuario.EnderecoEntrega.Pais);
                    if (string.IsNullOrEmpty(usuario.EnderecoEntrega.Observacao))
                        comandoEndereco.Parameters.AddWithValue("@Observacao", DBNull.Value);
                    else
                        comandoEndereco.Parameters.AddWithValue("@Observacao", usuario.EnderecoEntrega.Observacao);
                    idsEnderecos.Add(Convert.ToInt32(comandoEndereco.ExecuteScalar()));
                }
                else
                {
                    comandoEndereco.Parameters.AddWithValue("@TipoEndereco", usuario.EnderecoCobranca.TipoEndereco);
                    comandoEndereco.Parameters.AddWithValue("@TipoResidencia", usuario.EnderecoCobranca.TipoResidencia);
                    comandoEndereco.Parameters.AddWithValue("@TipoLogradouro", usuario.EnderecoCobranca.TipoLogradouro);
                    comandoEndereco.Parameters.AddWithValue("@Logradouro", usuario.EnderecoCobranca.Logradouro);
                    comandoEndereco.Parameters.AddWithValue("@Cep", usuario.EnderecoCobranca.Cep);
                    comandoEndereco.Parameters.AddWithValue("@Numero", usuario.EnderecoCobranca.Numero);
                    comandoEndereco.Parameters.AddWithValue("@Bairro", usuario.EnderecoCobranca.Bairro);
                    comandoEndereco.Parameters.AddWithValue("@Cidade", usuario.EnderecoCobranca.Cidade);
                    comandoEndereco.Parameters.AddWithValue("@Estado", usuario.EnderecoCobranca.Estado);
                    comandoEndereco.Parameters.AddWithValue("@Pais", usuario.EnderecoCobranca.Pais);
                    if (string.IsNullOrEmpty(usuario.EnderecoCobranca.Observacao))
                        comandoEndereco.Parameters.AddWithValue("@Observacao", DBNull.Value);
                    else
                        comandoEndereco.Parameters.AddWithValue("@Observacao", usuario.EnderecoCobranca.Observacao);
                    idsEnderecos.Add(Convert.ToInt32(comandoEndereco.ExecuteScalar()));
                }

                cmdTextoEndereco = "INSERT INTO UsuariosEnderecos(UsuarioId," +
                                                            "EnderecoId" +
                                   ") " +
                                   "VALUES(@UsuarioId," +
                                          "@EnderecoId" +
                                   ")";
                comandoEndereco = new SqlCommand(cmdTextoEndereco, conexao, transacao);

                comandoEndereco.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                comandoEndereco.Parameters.AddWithValue("@EnderecoId", idsEnderecos[0]);
                comandoEndereco.ExecuteNonQuery();
                comandoEndereco.Parameters.Clear();
                comandoEndereco.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                comandoEndereco.Parameters.AddWithValue("@EnderecoId", idsEnderecos[1]);
                comandoEndereco.ExecuteNonQuery();
                comandoEndereco.Dispose();

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
            Usuario usuario = (Usuario)entidade;
            string cmdTextoUsuario;

            try
            {
                Conectar();
                BeginTransaction();

                SqlCommand comandoUsuario;

                if (usuario.CartaoPreferencial > 0)
                {
                    cmdTextoUsuario = "UPDATE Usuarios SET CartaoPreferencial = @CartaoPreferencial WHERE UsuarioId = @UsuarioId";
                    comandoUsuario = new SqlCommand(cmdTextoUsuario, conexao, transacao);
                    comandoUsuario.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                    comandoUsuario.Parameters.AddWithValue("@CartaoPreferencial", usuario.CartaoPreferencial);
                    comandoUsuario.ExecuteNonQuery();
                }
                else
                {
                    cmdTextoUsuario = "UPDATE Usuarios SET Nome = @Nome," +
                                                          "Sexo = @Sexo," +
                                                          "DataNascimento = @DataNascimento," +
                                                          "Cpf = @Cpf," +
                                                          "TelefoneTipo = @TelefoneTipo," +
                                                          "TelefoneDdd = @TelefoneDdd," +
                                                          "TelefoneDdi = @TelefoneDdi," +
                                                          "TelefoneNumero = @TelefoneNumero," +
                                                          "Email = @Email" +
                                      "WHERE UsuarioId = @UsuarioId";

                    comandoUsuario = new SqlCommand(cmdTextoUsuario, conexao, transacao);

                    comandoUsuario.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                    comandoUsuario.Parameters.AddWithValue("@Nome", usuario.NomeCompleto);
                    comandoUsuario.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    comandoUsuario.Parameters.AddWithValue("@DataNascimento", usuario.DataNascimento);
                    comandoUsuario.Parameters.AddWithValue("@Cpf", usuario.Cpf);
                    comandoUsuario.Parameters.AddWithValue("@TelefoneTipo", usuario.TelefoneTipo);
                    comandoUsuario.Parameters.AddWithValue("@TelefoneDdd", usuario.TelefoneDdd);
                    comandoUsuario.Parameters.AddWithValue("@TelefoneNumero", usuario.TelefoneNumero);
                    comandoUsuario.Parameters.AddWithValue("@Email", usuario.Email);
                    comandoUsuario.ExecuteNonQuery();
                }

                Commit();
                comandoUsuario.Dispose();
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

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            Usuario usuario = (Usuario)entidade;
            List<Usuario> usuarios;
            string cmdTextoUsuario = "";

            try
            {
                Conectar();

                if (usuario.Id == 0 && string.IsNullOrEmpty(usuario.NomeCompleto))
                    cmdTextoUsuario = "SELECT * FROM Usuarios";
                else if (usuario.Id != 0 && string.IsNullOrEmpty(usuario.NomeCompleto))
                    cmdTextoUsuario = "SELECT * FROM Usuarios WHERE UsuarioId = @UsuarioId";
                else if (usuario.Id != 0 && !string.IsNullOrEmpty(usuario.Senha))
                    cmdTextoUsuario = "SELECT * "
                                     + "FROM Usuarios " +
                                     "WHERE UsuarioId = @UsuarioId " +
                                     "AND Senha = CONVERT(varchar(32),HASHBYTES('SHA2_256', @Senha),1)";
                else if (usuario.Id == 0 && !string.IsNullOrEmpty(usuario.NomeCompleto))
                    cmdTextoUsuario = "SELECT * FROM Usuarios WHERE NomeCompleto like %@NomeCompleto%";

                SqlCommand comandoUsuario = new SqlCommand(cmdTextoUsuario, conexao);

                if (usuario.Id != 0 && string.IsNullOrEmpty(usuario.NomeCompleto))
                {
                    comandoUsuario.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                }
                else if (usuario.Id != 0 && !string.IsNullOrEmpty(usuario.Senha))
                {
                    comandoUsuario.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                    comandoUsuario.Parameters.AddWithValue("@Senha", usuario.Senha);
                }
                else if (usuario.Id == 0 && !string.IsNullOrEmpty(usuario.NomeCompleto))
                    comandoUsuario.Parameters.AddWithValue("@NomeCompleto", usuario.NomeCompleto);

                SqlDataReader drUsuario = comandoUsuario.ExecuteReader();
                comandoUsuario.Dispose();
                usuarios = DataReaderUsuarioParaList(drUsuario);
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
            return usuarios.ToList<EntidadeDominio>();
        }

        public List<Usuario> DataReaderUsuarioParaList(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows)
                return new List<Usuario>();

            List<Usuario> usuarios = new List<Usuario>();
            while (dataReader.Read())
            {
                try
                {
                    Usuario usuario = new Usuario
                    {
                        Id = Convert.ToInt32(dataReader["UsuarioId"]),
                        NomeCompleto = dataReader["NomeCompleto"].ToString(),
                        DataNascimento = dataReader["DataNascimento"].ToString(),
                        DataCadastro = Convert.ToDateTime(dataReader["DataCadastro"]),
                        TelefoneTipo = Convert.ToByte(dataReader["TelefoneTipo"]),
                        TelefoneDdd = dataReader["TelefoneDdd"].ToString(),
                        TelefoneNumero = dataReader["TelefoneNumero"].ToString(),
                        Cpf = (dataReader["Cpf"]).ToString(),
                        Email = dataReader["Email"].ToString(),
                        Senha = dataReader["Senha"].ToString()
                    };

                    usuarios.Add(usuario);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            dataReader.Close();

            return usuarios;
        }

    }
}
