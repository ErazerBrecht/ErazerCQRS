using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.DAL.ReadModel;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.Dapper.Repositories
{
    public class StatusRepository : BaseRepository, IStatusRepository
    {
        public StatusRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IList<StatusDto>> All()
        {
            using (var dbConnection = Connection)
            {
                const string query = @"SELECT * FROM Statuses";

                dbConnection.Open();
                var result = await dbConnection.QueryAsync<StatusDto>(query);
                return result.ToList();
            }
        }
    }
}
