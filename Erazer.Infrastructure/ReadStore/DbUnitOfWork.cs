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
    public class DbUnitOfWork: IDbUnitOfWork
    {
        private readonly IDbSession _dbSession;
        private readonly ILogger<DbUnitOfWork> _logger;

        public IDbRepository<StatusDto> Statuses { get; }
        public IDbRepository<PriorityDto> Priorities { get; }
        public IDbRepository<TicketListDto> TicketList { get; }
        public IDbRepository<TicketDto> Tickets { get; }
        public IDbRepository<SubscriptionDto> Subscriptions { get; }

        public DbUnitOfWork(IMongoDatabase db, IDbSession dbSession, ILogger<DbUnitOfWork> logger)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            _dbSession = dbSession ?? throw new ArgumentNullException(nameof(dbSession));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Statuses = new DbRepository<StatusDto>(db, dbSession);
            Priorities = new DbRepository<PriorityDto>(db, dbSession);
            TicketList = new DbRepository<TicketListDto>(db, dbSession);
            Tickets = new DbRepository<TicketDto>(db, dbSession);
            Subscriptions = new DbRepository<SubscriptionDto>(db, dbSession);
        }
        
        public Task Start()
        {
            return _dbSession.StartTransaction();
        }

        public async Task Commit()
        {
            try
            {
                await _dbSession.Commit();
            }
            catch (Exception e)
            {
                await _dbSession.Abort();
                
                _logger.LogError(e, "Error while trying to commit the transaction");
                throw;
            }
        }
    }
}