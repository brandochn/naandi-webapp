using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Data
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
