using System.Data.SqlClient;

namespace Core.Util
{
    public class ConexaoBd
    {
        public static SqlConnection GetConexao()
        {
            return new SqlConnection("Server=LOCALHOST\\SQLEXPRESS; Database=EpicBooks;Trusted_Connection=True;");
        }
    }
}
