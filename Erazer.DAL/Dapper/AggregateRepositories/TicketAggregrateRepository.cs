using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.Domain;
using Erazer.Services.Queries.DTOs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.Dapper.AggregateRepositories
{
    public class TicketAggregrateRepository : BaseAggregateRepository<Ticket>
    {
        public TicketAggregrateRepository(IMediator mediator, IConfiguration configuration) : base(mediator, configuration)
        {
        }

        public override async Task<Ticket> Get(string aggregateId)
        {
            using (var dbConnection = Connection)
            {
                const string query = @"SELECT [Id],
                                              [Title],
                                              [Description],
                                              [PriorityId],
                                              [StatusId]
                                       FROM TicketIV WITH (NOEXPAND)
                                       WHERE Id = @Id";

                dbConnection.Open();
                var result = await dbConnection.QueryAsync<Ticket>(query, new { Id = aggregateId });
                return result.FirstOrDefault();
            }
        }
    }
}
