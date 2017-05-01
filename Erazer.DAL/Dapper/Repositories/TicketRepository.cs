using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.DAL.ReadModel;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.Dapper.Repositories
{
    public class TicketRepository : BaseRepository, ITicketRepository
    {
        public TicketRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<TicketDto> Find(string id)
        {
            using (var dbConnection = Connection)
            {
                const string query = @"SELECT * FROM Tickets
                                       WHERE Id = @Id";

                dbConnection.Open();
                var result = await dbConnection.QueryAsync<TicketDto>(query, new {Id = id});
                return result.FirstOrDefault();
            }
        }

        public Task<TicketListDto> All()
        {
            throw new NotImplementedException();
        }
    }
}
