using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.Dapper.Repositories.Base
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;
        protected SqlConnection Connection => new SqlConnection(_connectionString);

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Erazer.Database");
        }
    }
}
