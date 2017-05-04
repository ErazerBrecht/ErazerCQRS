using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.DAL.ReadModel;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.Dapper.Repositories
{
    public class PriorityRepository : BaseRepository, IPriorityRepository
    {
        public PriorityRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IList<PriorityDto>> All()
        {
            using (var dbConnection = Connection)
            {
                const string query = @"SELECT * FROM Priorities";

                dbConnection.Open();
                var result = await dbConnection.QueryAsync<PriorityDto>(query);
                return result.ToList();
            }
        }
    }
}
