using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.ReadModel.Repositories
{
    public class TicketEventRepository : BaseRepository, ITicketEventQueryRepository
    {
        public TicketEventRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IList<TicketEventDto>> Find(string ticketId)
        {
            using (var dbConnection = Connection)
            {
                const string query = @"SELECT * 
                                       FROM [TicketEventsView]                                   
                                       WHERE TicketId = @Id
                                       ORDER BY Created DESC";

                dbConnection.Open();
                var result = await dbConnection.QueryAsync<TicketEventDto>(query, new { Id = ticketId});
                return result.ToList();
            }
        }
    }
}
