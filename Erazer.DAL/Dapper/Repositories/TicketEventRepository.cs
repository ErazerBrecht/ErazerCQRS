using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.DAL.Dapper.Repositories.Base;
using Erazer.DAL.ReadModel;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Erazer.DAL.Dapper.Repositories
{
    public class TicketEventRepository : BaseRepository, ITicketEventRepository
    {
        public TicketEventRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IList<TicketEventDto>> Find(string ticketId, int offset, int amount)
        {
            using (var dbConnection = Connection)
            {
                const string query = @"SELECT * 
                                       FROM TicketEvents                                   
                                       WHERE TicketId = @Id
                                       ORDER BY Created DESC
                                       OFFSET @OffSet ROWS FETCH NEXT @Amount ROWS ONLY";

                dbConnection.Open();
                var result = await dbConnection.QueryAsync<TicketEventDto>(query, new { Id = ticketId, OffSet = offset, Amount = amount });
                return result.ToList();
            }
        }
    }
}
