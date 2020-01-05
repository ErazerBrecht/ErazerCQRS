using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Erazer.Infrastructure.MongoDb
{
    public interface IDbSession: IDisposable
    {
        IClientSessionHandle Handle { get; }

        Task StartTransaction();
        Task Commit();
        Task Abort();
    }

    // TODO Fix Exceptions to more correct exceptions...
    public class MongoDbSession: IDbSession
    {
        private readonly IList<Func<Task>> _sideEffects;
        private readonly IMongoDatabase _mongoDb;
        private readonly ILogger<MongoDbSession> _logger;
        private IClientSessionHandle _mongoSession;

        public IClientSessionHandle Handle
        {
            get
            {
                if(_mongoSession == null)
                    throw new Exception("Transaction is not started yet!");

                return _mongoSession;
            }
        }

        public MongoDbSession(IMongoDatabase mongoDb, ILogger<MongoDbSession> logger)
        {
            _mongoDb = mongoDb;
            _logger = logger;
            _sideEffects = new List<Func<Task>>();
        }

        public void Dispose()
        {
            _logger.LogTrace("Stopping MongoDb session");
            _mongoSession?.Dispose();
            _logger.LogDebug("MongoDb session stopped");
        }

        public async Task StartTransaction()
        {
            _logger.LogTrace("Starting MongoDb session");
            _mongoSession = await _mongoDb.Client.StartSessionAsync();
            _logger.LogTrace("MongoDb session started");

            _logger.LogTrace("Starting MongoDb transaction");
            _mongoSession.StartTransaction();
            _logger.LogDebug("MongoDb transaction started");
        }

        public Task Commit()
        {
           if (_mongoSession?.IsInTransaction != true)
               throw new Exception("Cannot commit when there is no transaction started!");

           return _mongoSession.CommitTransactionAsync();
        }

        public Task Abort()
        {
            if (_mongoSession?.IsInTransaction != true)
                throw new Exception("Cannot commit when there is no transaction started!");

            return _mongoSession.AbortTransactionAsync();
        }
    }
}
