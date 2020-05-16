using MySql.Data.MySqlClient;

namespace Naandi.Shared.DataBase
{
    public class ApplicationDbContext
    {
        private string ConnectionString { get; set; }

        public ApplicationDbContext(string _connectionString)
        {
            ConnectionString = _connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}