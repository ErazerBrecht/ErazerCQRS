using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.Dapper.QueryRepositories
{
    public class StatusQueryRepository : BaseRepository, IStatusQueryRepository
    {
        public StatusQueryRepository(IConfiguration configuration) : base(configuration)
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
