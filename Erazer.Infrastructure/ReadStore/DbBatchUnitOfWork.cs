using System;
using System.Threading.Tasks;
using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Detail;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore
{
    public class DbBatchUnitOfWork : IDbUnitOfWork
    {
        private readonly IDbSession _dbSession;
        private readonly ILogger<DbBatchUnitOfWork> _logger;

        public IDbRepository<StatusDto> Statuses { get; }
        public IDbRepository<PriorityDto> Priorities { get; }
        public IDbRepository<TicketListDto> TicketList { get; }
        public IDbRepository<TicketDto> Tickets { get; }
        public IDbRepository<SubscriptionDto> Subscriptions { get; }

        public DbBatchUnitOfWork(IMongoDatabase db, IDbSession dbSession, ILogger<DbBatchUnitOfWork> logger)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            _dbSession = dbSession ?? throw new ArgumentNullException(nameof(dbSession));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Statuses = new DbBatchRepository<StatusDto>(db);
            Priorities = new DbBatchRepository<PriorityDto>(db);
            TicketList = new DbBatchRepository<TicketListDto>(db);
            Tickets = new DbBatchRepository<TicketDto>(db);
            Subscriptions = new DbBatchRepository<SubscriptionDto>(db);
        }

        public Task Start()
        {
            throw new NotSupportedException("Batch UOW -> Doesn't support starting a transaction");
        }

        public async Task Commit()
        {
            try
            {
                await _dbSession.StartTransaction();
                await ((DbBatchRepository<StatusDto>) Statuses).Flush(_dbSession);
                await ((DbBatchRepository<PriorityDto>) Priorities).Flush(_dbSession);
                await ((DbBatchRepository<TicketListDto>) TicketList).Flush(_dbSession);
                await ((DbBatchRepository<TicketDto>) Tickets).Flush(_dbSession);
                await ((DbBatchRepository<SubscriptionDto>) Subscriptions).Flush(_dbSession);

                await _dbSession.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while 'flushing and commiting' batch UOW");
                await _dbSession.Abort();

                throw;
            }
        }
    }
}