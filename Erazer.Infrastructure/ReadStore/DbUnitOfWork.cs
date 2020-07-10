using System;
using System.Threading.Tasks;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Syncing.Infrastructure;
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
        public IDbRepository<TicketEventDto> TicketEvents { get; }
        private IDbRepository<PositionDto> Position { get; }

        public DbUnitOfWork(IMongoDatabase db, IDbSession dbSession, ILogger<DbUnitOfWork> logger)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            _dbSession = dbSession ?? throw new ArgumentNullException(nameof(dbSession));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Statuses = new DbRepository<StatusDto>(db, dbSession);
            Priorities = new DbRepository<PriorityDto>(db, dbSession);
            TicketList = new DbRepository<TicketListDto>(db, dbSession);
            Tickets = new DbRepository<TicketDto>(db, dbSession);
            TicketEvents = new DbRepository<TicketEventDto>(db, dbSession);
            Position = new DbRepository<PositionDto>(db, dbSession);
        }
        
        public Task Start()
        {
            return _dbSession.StartTransaction();
        }

        public async Task Commit(long position)
        {
            var newPosition = new PositionDto
            {
                Id = "ERAZER_CQRS_SUBSCRIPTION_POSITION",
                CheckPoint = position,
                UpdatedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
                
            try
            {
                await Position.Mutate(newPosition);
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