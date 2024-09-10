using EdTech.Interfaces;
using System.Data;
using System.Data.SqlClient;


namespace EdTech.Utils

{
    public class ConnectionProvider : Interfaces.IConnectionProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
