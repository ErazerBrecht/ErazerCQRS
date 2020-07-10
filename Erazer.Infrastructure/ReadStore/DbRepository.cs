using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.DTO;
using Erazer.Infrastructure.MongoDb;
using Erazer.Syncing.Infrastructure;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore
{
    public class DbRepository<T>: IDbRepository<T> where T : class, IDto
    {
        private readonly IDbSession _session;
        private readonly IMongoCollection<T> _collection;

        public DbRepository(IMongoDatabase db, IDbSession session)
        {
            if (db == null ) throw new ArgumentNullException(nameof(db));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            
            var factory = new MongoDbCollectionFactory<T>(db);
            _collection = factory.Build();
        }

        public Task<T> Find(string id, CancellationToken cancellationToken = default)
        {
            return Find(x => x.Id == id, cancellationToken);
        }

        public Task<T> Find(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            return _collection.Find(_session.Handle, filter).SingleOrDefaultAsync(cancellationToken);
        }

        public Task Add(T newDocument, CancellationToken cancellationToken = default)
        {
            return _collection.InsertOneAsync(_session.Handle, newDocument, cancellationToken: cancellationToken);
        }

        public Task Mutate(T newValue, CancellationToken cancellationToken = default)
        {
            return _collection.ReplaceOneAsync(_session.Handle, x => x.Id == newValue.Id, newValue, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }

        public Task Delete(string id, CancellationToken cancellationToken = default)
        {
            return Delete(x => x.Id == id, cancellationToken);
        }

        public Task Delete(T model, CancellationToken cancellationToken = default)
        {
            return Delete(x => x.Id == model.Id, cancellationToken: cancellationToken);
        }

        public Task Delete(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            return _collection.DeleteOneAsync(_session.Handle, filter, cancellationToken: cancellationToken);
        }
    }
}